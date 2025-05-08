#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import librosa
import numpy as np
import json
import argparse
from scipy.signal import find_peaks

def detect_drum_beats(audio_file, output_json, threshold=0.2, min_interval=0.2, frame_length=512):
    """
    Advanced drum beat detector with kick/snare separation and accent detection
    
    Args:
        audio_file: Input audio file path
        output_json: Output JSON file path
        threshold: Intensity threshold (0-1)
        min_interval: Minimum time between beats (seconds)
        frame_length: FFT window size
    """
    try:
        # Load audio with optimized settings for drum detection
        y, sr = librosa.load(audio_file, sr=44100)  # Fixed sample rate for consistency
        
        # Harmonic-percussive separation with enhanced settings
        y_harmonic, y_percussive = librosa.effects.hpss(y, margin=(3.0, 6.0))
        
        # Spectral flux for accent detection
        spectral_flux = librosa.onset.onset_strength(
            y=y_percussive, 
            sr=sr,
            hop_length=frame_length//2,
            n_fft=frame_length
        )
        
        # Enhanced STFT analysis
        S = np.abs(librosa.stft(y_percussive, n_fft=frame_length, hop_length=frame_length//2))
        freqs = librosa.fft_frequencies(sr=sr, n_fft=frame_length)
        times = librosa.frames_to_time(np.arange(S.shape[1]), sr=sr, hop_length=frame_length//2)
        
        # Frequency ranges for drum types
        kick_range = (freqs >= 50) & (freqs <= 150)
        snare_range = (freqs >= 150) & (freqs <= 500)
        
        # Energy envelopes
        kick_energy = np.sum(S[kick_range, :], axis=0)
        snare_energy = np.sum(S[snare_range, :], axis=0)
        
        # Normalize energies
        kick_energy = librosa.util.normalize(kick_energy)
        snare_energy = librosa.util.normalize(snare_energy)
        combined_energy = (kick_energy + snare_energy) / 2
        
        # Convert min_interval to frames
        min_interval_frames = int(min_interval * sr / (frame_length // 2))
        
        # Detect peaks with advanced settings
        peaks, properties = find_peaks(
            combined_energy,
            height=threshold,
            distance=min_interval_frames,
            prominence=(0.3, None),
            width=(1, 10),
            rel_height=0.5
        )
        
        # Classify and collect beats
        beats = []
        for i, p in enumerate(peaks):
            time = times[p]
            
            # Determine drum type
            if np.max(S[kick_range, p]) > np.max(S[snare_range, p]):
                drum_type = "kick"
                intensity = kick_energy[p]
            else:
                drum_type = "snare"
                intensity = snare_energy[p]
            
            # Detect accent using spectral flux
            is_accent = spectral_flux[p] > np.percentile(spectral_flux, 75)
            
            beats.append({
                "time": round(float(time), 3),
                "type": drum_type,
                "intensity": float(intensity),
                "accent": bool(is_accent),
                "spectral_flux": float(spectral_flux[p])
            })
        
        # Save comprehensive results
        result = {
            "beats": beats,
            "bpm": float(librosa.beat.tempo(
                onset_envelope=combined_energy,
                sr=sr,
                hop_length=frame_length//2
            )[0]),
            "analysis_settings": {
                "sample_rate": sr,
                "frame_length": frame_length,
                "hop_length": frame_length//2,
                "threshold": threshold,
                "min_interval": min_interval,
                "kick_freq_range": [50, 150],
                "snare_freq_range": [150, 500]
            }
        }
        
        with open(output_json, 'w', encoding='utf-8') as f:
            json.dump(result, f, indent=2)
        
        print(f"Detected {len(beats)} drum beats (BPM: {result['bpm']:.1f})")
        print(f"Results saved to {output_json}")
        return True

    except Exception as e:
        print(f"Error: {str(e)}")
        return False

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description='Advanced Drum Beat Detector')
    parser.add_argument('input', help='Input audio file')
    parser.add_argument('output', help='Output JSON file')
    parser.add_argument('--threshold', type=float, default=0.2,
                      help='Intensity threshold (0-1, default: 0.2)')
    parser.add_argument('--min-interval', type=float, default=0.2,
                      help='Minimum time between beats in seconds (default: 0.2)')
    
    args = parser.parse_args()
    
    detect_drum_beats(
        audio_file=args.input,
        output_json=args.output,
        threshold=args.threshold,
        min_interval=args.min_interval
    )