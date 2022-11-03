using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParentGameObject : MonoBehaviour
{
    // Start is called before the first frame update
    public void DestroyParentObject() {
        Destroy(this.gameObject.transform.parent.gameObject);
    }
}
