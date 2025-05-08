using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvMng : PersistentSingleton<EnvMng>
{
    public List<Env> listEnv;
    public void InitMap(int typeEnv, int indexEnv)
    {
        if (listEnv[typeEnv].envs.Count > (indexEnv - 1))
            Instantiate(listEnv[typeEnv].envs[indexEnv], Vector3.zero, Quaternion.identity);
    }    
}
[System.Serializable]
public class Env
{
    public List<GameObject> envs;
}
