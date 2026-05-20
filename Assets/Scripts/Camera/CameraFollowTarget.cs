using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public CameraType cameraType = CameraType.None;
    //public bool isBattle;
    //public bool inBattleScene;
    public Transform player;
    public Vector3 roomPos;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (inBattleScene)
        //{
        //    if (isBattle)
        //    {

        //        //transform.position = NManager.Instance.player.playerInstance.currentRoom.Value.roomObject.Value.transform.position;
        //        transform.position = roomPos;

        //    }
        //    else
        //    {
        //        Vector2 mouse = Input.mousePosition;
        //        Vector2 center = new Vector2(Screen.width, Screen.height) * 0.5f;

        //        Vector2 dir = (mouse - center) / center;

        //        Vector3 offset = new Vector3(dir.x, dir.y, 0) * 2f;

        //        transform.position = player.position + offset;
        //    }
        //}
        //else
        //{
        //    transform.position = Vector3.zero;
        //}

        if (cameraType == CameraType.None)
        {
            transform.position = Vector3.zero;
        }
        if (cameraType == CameraType.Battle)
        {
            // transform.position = roomPos;
            Vector2 mouse = Input.mousePosition;
            Vector2 center = new Vector2(Screen.width, Screen.height) * 0.5f;

            Vector2 dir = (mouse - center) / center;

            Vector3 offset = new Vector3(dir.x, dir.y, 0) * 2f;

            transform.position = roomPos + offset;
        }
        if (cameraType == CameraType.Map)
        {
            Vector2 mouse = Input.mousePosition;
            Vector2 center = new Vector2(Screen.width, Screen.height) * 0.5f;

            Vector2 dir = (mouse - center) / center;

            Vector3 offset = new Vector3(dir.x, dir.y, 0) * 2f;

            transform.position = player.position + offset;
        }
    }
}
