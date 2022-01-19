using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    GameObject board;

    bool isMove, isCombine;
    int boxPosX, boxPosZ;

    // Box Instantiate Vector and Position Value
    const float boxMaxPos = 3.05f, boxSectionPos = 2.016f, boxHeight = -2.15f;

    void Start()
    {
        this.board = GameObject.Find("2048Board");
    }
    void Update()
    {
        if (isMove) Move(boxPosX, boxPosZ, isCombine);
    }

    public void Move(int posX, int posZ, bool combine)
    {
        Vector3 originalBoard;
        Vector3 finalPoint;
        // Board Position Save
        originalBoard = this.board.transform.eulerAngles;
        this.board.transform.eulerAngles = new Vector3(0, 0, 0);

        isMove = true;
        boxPosX = posX; boxPosZ = posZ;
        isCombine = combine;

        // Final Position Save
        finalPoint = new Vector3(boxMaxPos - posX * boxSectionPos, boxHeight, boxMaxPos - posZ * boxSectionPos);
        // Box Move
        transform.position = Vector3.MoveTowards(transform.position, finalPoint, 0.5f);
        // Box Position same Final Position Moving End
        if (transform.position == finalPoint)
        {
            isMove = false;
            if (combine)
            {
                isCombine = false;
                Destroy(gameObject);
            }
        }
        // Board Original Position Load
        this.board.transform.eulerAngles = originalBoard;
    }
}
