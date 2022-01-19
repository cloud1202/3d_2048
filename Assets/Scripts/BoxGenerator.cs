
using UnityEngine;

public class BoxGenerator : MonoBehaviour
{
    // Board and GameManager GameObject
    public GameObject board;
    public GameObject gameManager;

    // Box Prefabs Repository
    [HideInInspector]
    public GameObject[,] box = new GameObject[4, 4];
    public GameObject[] boxPrefabs;

    // Box Instantiate Vector and Position Value
    const float boxMaxPos = 3.05f, boxSectionPos = 2.016f, boxHeight = -2.15f;
    
    enum BoxState { IDLE, COMBINE, MOVE };
    BoxState boxState = BoxState.IDLE;

    public bool deleteTag()
    {
        bool isFull = true;
        for (int row = 0; row <= 3; row++)
        {
            for (int col = 0; col <= 3; col++)
            {
                if (box[row, col] == null) { isFull = false; continue; }
                if (box[row, col].tag == "Combine") box[row, col].tag = "Untagged";
            }
        }
        return isFull;
    }
    public void emptySpace(bool isFull)
    {
        bool isEmpty = false;
        if (isFull)
        {
            for (int col = 0; col <= 2; col++) { 
                for (int row = 0; row <= 2 - col; row++) {
                    if (box[row, col].name == box[row + 1, col].name) isEmpty = true;
                    if (box[row, col].name == box[row, col + 1].name) isEmpty = true;

                    if (box[3 - row, 3 - col].name == box[2 - row, 3 - col].name) isEmpty = true;
                    if (box[3 - row, 3 - col].name == box[3 - row, 2 - col].name) isEmpty = true;
                }
            }
            if (!isEmpty) { gameManager.GetComponent<GameManager>().GameEnd(); }
        }
    }

    public void spawn()
    {
        int boardRow, boardCol;
        Vector3 originalBoard = this.board.transform.eulerAngles;
        this.board.transform.eulerAngles = new Vector3(0, 0, 0);
        while (true) { boardRow = Random.Range(0, 4); boardCol = Random.Range(0, 4); if (box[boardRow, boardCol] == null) break; }
        box[boardRow, boardCol] = Instantiate(Random.Range(0, 10) > 0 ? boxPrefabs[0] : boxPrefabs[1], new Vector3(boxMaxPos - boardRow * boxSectionPos, boxHeight, boxMaxPos - boardCol * boxSectionPos), Quaternion.identity);
        box[boardRow, boardCol].transform.parent = this.board.transform;
        this.board.transform.eulerAngles = originalBoard;
    }
    public void MoveOrCombine(int firstRow, int firstCol, int finalRow, int finalCol)
    {
        int boxIndex;
        CombineLogic(firstRow, firstCol, finalRow, finalCol);
        if (boxState == BoxState.IDLE)
        {
            return;
        }
        board.GetComponent<BoardManager>().isMove = true;
        if (boxState == BoxState.MOVE)
        {
            box[firstRow, firstCol].GetComponent<BoxManager>().Move(finalRow, finalCol, false);
            box[finalRow, finalCol] = box[firstRow, firstCol];
            box[firstRow, firstCol] = null;
        }
        else
        {
            for (boxIndex = 0; boxIndex < box.Length; boxIndex++) if (box[finalRow, finalCol].name == boxPrefabs[boxIndex].name + "(Clone)") break;
            box[firstRow, firstCol].GetComponent<BoxManager>().Move(finalRow, finalCol, true);
            Destroy(box[finalRow, finalCol]);
            box[firstRow, firstCol] = null;
            Combine(finalRow, finalCol, boxIndex);
            box[finalRow, finalCol].tag = "Combine";
        }
    }
    void Combine(int combineRow, int combineCol, int combineIndex)
    {
        Vector3 originalBoard = this.board.transform.eulerAngles;
        this.board.transform.eulerAngles = new Vector3(0, 0, 0);
        box[combineRow, combineCol] = Instantiate(boxPrefabs[combineIndex + 1], new Vector3(boxMaxPos - combineRow * boxSectionPos, boxHeight, boxMaxPos - combineCol * boxSectionPos), Quaternion.identity);
        this.gameManager.GetComponent<GameManager>().AddScore(combineIndex + 1);
        box[combineRow, combineCol].transform.parent = this.board.transform;
        this.board.transform.eulerAngles = originalBoard;
    }
    void CombineLogic(int firstRow, int firstCol, int finalRow, int finalCol)
    {
        if (box[finalRow, finalCol] == null && box[firstRow, firstCol] != null)
        {
            boxState = BoxState.MOVE;
            return;
        }

        if (box[firstRow, firstCol] != null && box[finalRow, finalCol] != null && box[firstRow, firstCol].name == box[finalRow, finalCol].name && box[firstRow, firstCol].tag != "Combine" && box[finalRow, finalCol].tag != "Combine")
        {
            boxState = BoxState.COMBINE;
            return;
        }
        boxState = BoxState.IDLE;
        return;
    }
}