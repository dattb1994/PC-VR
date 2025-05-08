#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import librosa
import numpy as np
import json
import argparse
from scipy.signal import find_peaks

def detect_heavy_bass_beats(audio_file, output_json, threshold=0.3, min_interval=0.5, frame_length=2048):
    """
    Detect heavy bass beats with minimum interval between beats
    
    Args:
        audio_file: Input audio file path
        output_json: Output JSON file path
        threshold: Intensity threshold (0-1)
        min_interval: Minimum time between beats (seconds)
        frame_length: FFT window size
    """
    try:
        # Load audio with extended low frequency response
        y, sr = librosa.load(audio_file, sr=None)
        
        # Strong bass emphasis
        y_bass = librosa.effects.preemphasis(y, coef=0.98)
        
        # Focus on sub-bass frequencies (20-100Hz)
        S = np.abs(librosa.stft(y_bass, n_fft=frame_length))
        freqs = librosa.fft_frequencies(sr=sr, n_fft=frame_length)
        sub_bass = (freqs >= 20) & (freqs <= 100)
        S_bass = S[sub_bass, :]
        
        # Create energy envelope
        energy = np.sum(S_bass, axis=0)
        energy = librosa.util.normalize(energy)
        
        # Convert min_interval to samples
        min_interval_frames = int(min_interval * sr / (frame_length // 2))
        
        # Advanced peak detection
        peaks, _ = find_peaks(
            energy,
            height=threshold,
            distance=min_interval_frames,
            prominence=0.2,
            width=3
        )
        
        # Convert to timestamps
        beat_times = [round(float(librosa.frames_to_time(p, sr=sr, hop_length=frame_length//2)), 3) 
                    for p in peaks if energy[p] > threshold]
        
        # Save results in Unity-compatible format
        result = {
            "beats": beat_times,
            "bpm": None,  # Có thể thêm tính năng detect BPM sau
            "audio_file": audio_file,
            "detection_settings": {
                "threshold": threshold,
                "min_interval": min_interval,
                "frame_length": frame_length
            }
        }
        
        with open(output_json, 'w', encoding='utf-8') as f:
            json.dump(result, f, indent=2)
        
        print(f"Detected {len(beat_times)} heavy bass beats (min interval: {min_interval}s)")
        print(f"Results saved to {output_json}")
        return True

    except Exception as e:
        print(f"Error: {str(e)}")
        return False

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description='Heavy Bass Beat Detector')
    parser.add_argument('input', help='Input audio file')
    parser.add_argument('output', help='Output JSON file')
    parser.add_argument('--threshold', type=float, default=0.3,
                      help='Intensity threshold (0-1, default: 0.3)')
    parser.add_argument('--min-interval', type=float, default=0.5,
                      help='Minimum time between beats in seconds (default: 0.5)')
    
    args = parser.parse_args()
    
    detect_heavy_bass_beats(
        audio_file=args.input,
        output_json=args.output,
        threshold=args.threshold,
        min_interval=args.min_interval
    )