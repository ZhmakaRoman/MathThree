using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomManager : MonoBehaviour
{
    public int GetRandomNonMatchingElement(GameObject[,] gridElements,GameObject[]elements,int row, int column)
    {
        int dotToUse = Random.Range(0, elements.Length);

        // Проверяется условие, где column представляет текущую колонку, и мы проверяем, что она больше или равна 2
        //  Затем сравниваются теги элементов _gridElements[row, column - 1] и _gridElements[row, column - 2]
        // с тегом элемента _elements[dotToUse]. Если оба сравнения истинны (т.е., если есть горизонтальное совпадение),
        // выполняется следующий блок кода.
        if (column >= 2 && gridElements[row, column - 1].tag == elements[dotToUse].tag &&
            gridElements[row, column - 2].tag == elements[dotToUse].tag)
        {
            // Если есть горизонтальное совпадение, индекс dotToUse инкрементируется на 1 и затем делится по модулю
            //на _elements.Length. Это позволяет выбрать следующий элемент в круговом порядке (если dotToUse было
            //последним индексом, оно перейдет к 0).
            dotToUse = (dotToUse + 1) % elements.Length;
        }

        //  Здесь проверяется условие, где row представляет текущую строку, и мы проверяем, что она больше или равна 2
        //  Затем сравниваются теги элементов _gridElements[row - 1, column] и _gridElements[row - 2, column] с тегом
        // элемента _elements[dotToUse]. Если оба сравнения истинны (т.е., если есть вертикальное совпадение),
        // выполняется  код.
        if (row >= 2 && gridElements[row - 1, column].tag == elements[dotToUse].tag &&
            gridElements[row - 2, column].tag == elements[dotToUse].tag)
        {
            // Если есть вертикальное совпадение, индекс dotToUse снова инкрементируется на 1 и
            // делится по модулю на _elements.Length, чтобы выбрать следующий элемент в круговом порядке.
            dotToUse = (dotToUse + 1) % elements.Length; 
        }

        return dotToUse;
    }
}
