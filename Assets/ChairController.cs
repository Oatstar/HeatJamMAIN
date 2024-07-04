using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairController : MonoBehaviour
{
    [SerializeField] int chairId;

    public int GetChairId()
    {
        return chairId;
    }
}
