using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject boxGenerator;

    const int startBoxCount = 2;

    bool isTouch, isTop = true, isBottom = true, isRight = true, isLeft = true;
    public bool isMove = false;

    Vector3 firstTap, gap;
    const float speed = 100.0f;

    [HideInInspector]
    public bool boardRotation;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isTouch = true;
            firstTap = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            gap = Input.mousePosition - firstTap;

            // 움직인 거리가 짧으면 리턴
            if (gap.magnitude < 20) return;
            gap.Normalize();

            // 움직이지 않으면 실행하지 않음
            if (isTouch)
            {
                //Up
                if (gap.y > 0 && gap.x > -0.5f && gap.x < 0.5f)
                {
                    if (isTop)
                    {
                        this.gameObject.transform.eulerAngles += new Vector3(gap.y * Time.deltaTime * speed, 0.0f, this.gameObject.transform.rotation.z);
                    }
                    else
                    {
                        for (int row = 0; row <= 3; row++)
                        {
                            for (int col = 3; col >= 1; col--)
                            {
                                for (int index = 0; index <= col - 1; index++)
                                {
                                    boxGenerator.GetComponent<BoxGenerator>().MoveOrCombine(row, index + 1, row, index);
                                }
                            }
                        }
                        isTouch = false;
                    }
                }
                // Down
                else if (gap.y < 0 && gap.x > -0.5f && gap.x < 0.5f)
                {
                    if (isBottom)
                    {
                        this.gameObject.transform.eulerAngles += new Vector3(gap.y * Time.deltaTime * speed, 0.0f, this.gameObject.transform.rotation.z);
                    }
                    else
                    {
                        for (int row = 0; row <= 3; row++)
                        {
                            for (int col = 0; col <= 2; col++)
                            {
                                for (int index = 3; index >= col + 1; index--)
                                {
                                    boxGenerator.GetComponent<BoxGenerator>().MoveOrCombine(row, index - 1, row, index);
                                }
                            }
                        }
                        isTouch = false;
                    }
                }
                //Right
                else if (gap.x > 0 && gap.y > -0.5f && gap.y < 0.5f)
                {
                    if (isRight)
                    {
                        this.gameObject.transform.eulerAngles += new Vector3(this.gameObject.transform.rotation.x, 0.0f, gap.x * Time.deltaTime * -speed);
                    }
                    else
                    {
                        for (int col = 0; col <= 3; col++)
                        {
                            for (int row = 3; row >= 1; row--)
                            {
                                for (int index = 0; index <= row - 1; index++)
                                {
                                    boxGenerator.GetComponent<BoxGenerator>().MoveOrCombine(index + 1, col, index, col);
                                }
                            }
                        }
                        isTouch = false;
                    }
                }
                //Left
                else if (gap.x < 0 && gap.y > -0.5f && gap.y < 0.5f)
                {
                    if (isLeft)
                    {
                        this.gameObject.transform.eulerAngles += new Vector3(this.gameObject.transform.rotation.x, 0.0f, gap.x * Time.deltaTime * -speed);

                    }
                    else
                    {
                        for (int col = 0; col <= 3; col++)
                        {
                            for (int row = 0; row <= 2; row++)
                            {
                                for (int index = 3; index >= row + 1; index--)
                                {
                                    boxGenerator.GetComponent<BoxGenerator>().MoveOrCombine(index - 1, col, index, col);
                                }
                            }
                        }
                        isTouch = false;
                    }
                }
                if (isMove)
                {
                    isMove = false;
                    boxGenerator.GetComponent<BoxGenerator>().spawn();

                    boxGenerator.GetComponent<BoxGenerator>().emptySpace(boxGenerator.GetComponent<BoxGenerator>().deleteTag());
                }
            }
        }
    }
    public void GameStart()
    {
        this.boardRotation = true;
        StartCoroutine(BoardRotate());
    }

    IEnumerator BoardRotate()
    {
        // When the Game Starts board rotates
        while (this.boardRotation)
        {
            gameObject.transform.Rotate(Vector3.back * Time.deltaTime * 180.0f);
            if (gameObject.transform.eulerAngles.z > 180.0f)
            {
                boardRotation = false;
                StartGameSpawnBox();
            }
            yield return new WaitForSeconds(0.0001f);
        }
    }

    void StartGameSpawnBox()
    {
        StopCoroutine(BoardRotate());
        for (int count = 0; count < startBoxCount; count++)
        {
            this.boxGenerator.GetComponent<BoxGenerator>().spawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TopSensor")
        {
            isTop = false;
        }
        if (other.gameObject.name == "LeftSensor")
        {
            isLeft = false;
        }
        if (other.gameObject.name == "BottomSensor")
        {
            isBottom = false;
        }
        if (other.gameObject.name == "RightSensor")
        {
            isRight = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "TopSensor")
        {
            isTop = true;
        }
        if (other.gameObject.name == "LeftSensor")
        {
            isLeft = true;
        }
        if (other.gameObject.name == "BottomSensor")
        {
            isBottom = true;
        }
        if (other.gameObject.name == "RightSensor")
        {
            isRight = true;
        }
    }
}
