using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchBoardController : MonoBehaviour
{
    private GridTile _gridTile;
    private ElementRemovalMechanism _elementRemovalMechanism;
    private void Start()
    {
        _elementRemovalMechanism = GetComponent<ElementRemovalMechanism>();
        _gridTile = GetComponent<GridTile>();
    }
    // метод  заполняет  пустые ячейки в сетке новыми элементами.
    private void RefillBoardWithNewElements()
    {
        for (int i = 0; i < _gridTile.Width; i++)
        {
            for (int j = 0; j < _gridTile.Height; j++)
            {
                if (_gridTile.GridElements[i, j] == null)
                {
                    _gridTile.InitializeElements(i,j);
                }
            }
        }
    }
//метод проверяет наличия совпадений на игровой доске.
    private bool CheckForMatchesOnBoard()
    {
        for (int i = 0; i <  _gridTile.Width; i++)
        {
            for (int j = 0; j < _gridTile.Height; j++)
            {
                if (_gridTile.GridElements[i, j] != null)
                {
                    // Если элемент в текущей позиции совпадает
                    if (_gridTile.GridElements[i, j].GetComponent<ElementContoller>().IsMatched)
                    {
                        return true;// Возвращаем true для указания наличия совпадений на доске
                    }
                }
            }
        }
        return false; // Если  false  указывает на отсутствия совпадений на доске
    }
//метод  заполнения сетки новыми элементами с последующей проверкой совпадений.
    public IEnumerator FillBoardWithMatchesCheck()
    {
        RefillBoardWithNewElements();// Заполняем сетку новыми элементами
        yield return new WaitForSeconds(0.5f);// Ждем 0.5 секунды  для заполнения сетки новыми элементами.
        while (CheckForMatchesOnBoard())// Пока на доске есть совпадения
        {
            yield return new WaitForSeconds(0.5f);
            //чтобы удалить совпадающие элементы и уменьшить строки и столбцы.
            _elementRemovalMechanism.DestroyMatchedElementsAndDecreaseRowColumn();
        }
    }
}
