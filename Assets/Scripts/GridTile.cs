using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridTile : MonoBehaviour
{
     
     public GameObject[,] GridElements => _gridElements;
     public int Width => _width; 
     public int Height => _height;

     [SerializeField]private int _width;//ширина
     [SerializeField]private int _height;//высот
     [SerializeField]private GameObject[] _elements;//элементы которые будут располагаться в сетки
     [SerializeField]private GameObject _tilePrefab;
     private GameObject[,] _gridElements;//массив сетки для элементов
     private RandomManager _randomManager;
     private ElementRemovalMechanism _elementRemovalMechanism;

     private void Start()
     {
          _randomManager = GetComponent<RandomManager>();
          _gridElements = new GameObject[_width, _height];// заполняем массив элементов
          InitializeGridElements();
  }

  private void InitializeGridElements()
  {
       
       for (int i = 0; i < _width; i++)
       {
            for (int j = 0; j < _height; j++)
            {
                 Vector2 tempPosition = new Vector2(i, j);
                 InitializeBackTile(i,j);
                 InitializeElements(i,j);
            }
       }
  }

  private void InitializeBackTile(int i,int j)
  {
       GameObject grid = Instantiate(_tilePrefab, new Vector3(i,j,1), Quaternion.identity);
       grid.transform.parent = transform;
  }
  public void InitializeElements(int i,int j)
  {
       int elementToUse =   _randomManager.GetRandomNonMatchingElement(_gridElements, _elements, i, j);
       GameObject elements = Instantiate(_elements[elementToUse], new Vector3(i ,j ), Quaternion.identity);
       elements.GetComponent<ElementContoller>()._row = j;// Устанавливаем номер строки элемента
       elements.GetComponent<ElementContoller>()._colum = i;// Устанавливаем номер столбца элемента
       elements.transform.parent = transform;
       _gridElements[i, j] = elements;
  }
  
}
