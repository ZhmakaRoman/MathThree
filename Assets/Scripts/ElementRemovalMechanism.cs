using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementRemovalMechanism : MonoBehaviour
{
    private GridTile _gridTile;
    private MatchBoardController _matchBoardController;
    public Action OnSoundDestroyEvent;
    private void Start()
    {
        _matchBoardController = GetComponent<MatchBoardController>();
        _gridTile = GetComponent<GridTile>();
    }

    //метод отвечает за удаление элементов
    private void DestroyMatchedElement(int column, int row)
    {
        //Проверяем, является ли элемент в заданной позиции сетки совпадающим элементом
        if(_gridTile.GridElements[column,row].GetComponent<ElementContoller>().IsMatched)
        {
            Destroy(_gridTile.GridElements[column,row]);// Уничтожаем игровой объект элемента
            // Устанавливаем значение элемента в сетке как null, чтобы указать на его отсутствие
            _gridTile.GridElements[column, row] = null;
        }
    }
    //метод выполняет две основные задачи: уничтожение совпадающих элементов в сетке и запуск
    //процесса уменьшения строк и столбцов. 
    public void DestroyMatchedElementsAndDecreaseRowColumn()
    {
        for (int i = 0; i < _gridTile.Width; i++)
        {
            for (int j = 0; j < _gridTile.Height; j++)
            {
                if( _gridTile.GridElements[i,j] != null)
                {
                    DestroyMatchedElement(i,j);// Уничтожаем совпадающий элемент в заданной позиции
                    OnSoundDestroyEvent?.Invoke();
                }
            }
        }

        StartCoroutine(DecreaseRowColumnAndFillBoard());// Запускаем процесс уменьшения строк и столбцов
    }
    //метод по уменьшению строк и столбцов, заполнению сетки новыми элементами и проверке совпадений.
    private IEnumerator DecreaseRowColumnAndFillBoard()
    {
        int nullCount = 0;// Счетчик для подсчета удаленных элементов в столбце
        for (int i = 0; i < _gridTile.Width; i++)
        {
            for (int j = 0; j < _gridTile.Height; j++)
            {
                if(_gridTile.GridElements[i,j] == null)// Если элемент в текущей позиции сетки равен null (удаленный элемент)
                {
                    nullCount++;// Увеличиваем счетчик удаленных элементов в столбце
                }
                else if (nullCount > 0)// Если были удалены элементы в текущем столбце
                {
                    // Сдвигаем позицию элемента вниз на количество удаленных элементов
                    _gridTile.GridElements[i, j].GetComponent<ElementContoller>()._row -=nullCount;
                    // Устанавливаем значение элемента в сетке как null
                    _gridTile.GridElements[i, j] = null;
                }
            }

            nullCount = 0;// Сбрасываем счетчик удаленных элементов в столбце для следующего столбца
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(_matchBoardController.FillBoardWithMatchesCheck()); // Запускаем корутину для заполнения сетки новыми элементами
    }
}
