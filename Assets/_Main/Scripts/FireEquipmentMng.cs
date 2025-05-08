using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireEquipmentMng : Singleton<FireEquipmentMng>
{
    //public List<FireEquipment> equipments;
    public GameObject binhBot, binhCo2, binhNuoc, lang;
    public float lucDangBop = 0;
    public GameObject equipng = null;
    //private void Start()
    //{
    //    equipments = new List<FireEquipment>();
    //    foreach (var equip in FindObjectsByType<FireEquipment>(FindObjectsSortMode.None))
    //    {
    //        equip.ToggleActive(false);
    //        equipments.Add(equip);
    //    }
    //}
    //public FireEquipment GetByType<T>()
    //{                                    
    //    foreach (var fire in equipments)
    //    {
    //        if (fire.GetComponent<T>() != null)
    //            return fire;
    //    }
    //    return null;
    //}
    //public FireEquipment GetBinhByType(TypeBinh typeBinh)
    //{
    //    var binh = equipments.Find(x => x.GetComponent<Binh1>() && x.GetComponent<Binh1>().type == typeBinh);

    //    return binh;
    //}
}
