using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class ElementContoller : MonoBehaviour
{
    public bool IsMatched { get; private set; }// Показатель, совпадает ли объект
    public int _colum;// Текущая колонка
    public int _row;// Текущая строка
    private GameObject _otherElement;// Ссылка на другой объект
    private GridTile _gridElements;// Ссылка на игровую доску
    private ElementRemovalMechanism _elementRemovalMechanism; 
    private Camera _camera;// Ссылка на камеру
    private AnimationElementAppearance _animationElement; 
    private int _previousColum;// Предыдущая колонка
    private int _previousRow;// Предыдущая строка
    private float _swipeAngle = 0f;// Угол свайпа
    private float _swipeResist = 1f;
    private int _targetX;// Целевая позиция по X
    private int _targetY;// Целевая позиция по Y
  
    private Vector2 _firstTouchPosition;// Первая позиция касания
    private Vector2 _finalTouchPosition;// Конечная позиция касания
    private Vector2 _tempPosition;// Временная позиция
  
  private void Start()
  {
      _animationElement = GetComponent<AnimationElementAppearance>();
      _camera = Camera.main;// Получаем главную камеру
      _gridElements = FindObjectOfType<GridTile>();// Находим объект типа GridTie в сцене
      _elementRemovalMechanism = FindObjectOfType<ElementRemovalMechanism>();
  }

  private void Update()
  {
    
      FindMatches(); // Проверяем наличие совпадений
      if (IsMatched)
      {
          _animationElement.AnimationDeleteElement();
      }

      SmoothMoveToTarget();
  }

  private void SmoothMoveToTarget()
  {
      _targetX = _colum;// Устанавливаем целевую позицию по X равной текущей колонке
      _targetY = _row;// Устанавливаем целевую позицию по Y равной текущей строке
      // Если разница между текущей позицией по X и целевой позицией больше 0.1,
      // выполняем плавное перемещение
      if (MathF.Abs(_targetX - transform.position.x) > 0.1)
      {
          _tempPosition = new Vector2(_targetX, transform.position.y);
        MoveElementToTarget();
      }
      else
      {
          // Задаем точную позицию объекта, если он достаточно близок к целевой позиции
          _tempPosition = new Vector2(_targetX, transform.position.y);
          transform.position = _tempPosition;
      }
      if (MathF.Abs(_targetY - transform.position.y) > 0.1)
      {
          _tempPosition = new Vector2(transform.position.x, _targetY);
          MoveElementToTarget();
      }
      else
      {
          _tempPosition = new Vector2( transform.position.x,_targetY);
          transform.position = _tempPosition;
      }
  }
  
 private void MoveElementToTarget()
 {
     _animationElement.MoveElement(_tempPosition);
     if (_gridElements.GridElements[_colum, _row] != gameObject)
     {
         _gridElements.GridElements[_colum, _row] = gameObject;
     }
 }

 private void OnMouseDown()
 {
     _firstTouchPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
 }

 private void OnMouseUp()
 {
     _finalTouchPosition =_camera.ScreenToWorldPoint(Input.mousePosition);
     CalculateAngle();
 }

  private void CalculateAngle()
  {
      //В этой строке рассчитывается угол свайпа с помощью функции Math.Atan2().
      //Она принимает разность координат по оси Y  и разность координат по оси X  и возвращает угол в радианах.
      //Затем угол переводится в градусы и сохраняется в переменной 
      if (Mathf.Abs(_finalTouchPosition.y - _firstTouchPosition.y) > _swipeResist ||
          Mathf.Abs(_finalTouchPosition.x - _firstTouchPosition.x) > _swipeResist)
      {

          _swipeAngle = (float)(Math.Atan2(_finalTouchPosition.y - _firstTouchPosition.y,
              _finalTouchPosition.x - _firstTouchPosition.x) * 180 / Math.PI);
          MovePieces();
      }
  }
  // В этом методе проверяется, если _otherDot (ссылка на другую фишку) не равно null
  // (то  есть другая фишка, с которой был произведен обмен местами),
  // то выполняются дополнительные проверки.
  public IEnumerator ProcessMove()
  {
      //Если текущая фишка (this) и _otherDot не совпадают (!IsMatched) и _otherDot также не является
      //совпавшей фишкой (!_otherDot.GetComponent<Dot>().IsMatched), то происходит обратный обмен
      //позициями между текущей фишкой (this) и _otherDot.
      yield return new WaitForSeconds(0.5f);
      if (_otherElement != null)
      {
          if (!IsMatched && !_otherElement.GetComponent<ElementContoller>().IsMatched)
          {
              _otherElement.GetComponent<ElementContoller>()._row = _row;
              _otherElement.GetComponent<ElementContoller>()._colum = _colum;
              _row = _previousRow;
              _colum = _previousColum;
          }
          else
          {
              _elementRemovalMechanism.DestroyMatchedElementsAndDecreaseRowColumn();
            
          }
          _otherElement = null;
      }
  }
  //Метод перемещать игровую фишку в соответствии с рассчитанным углом свайпа.
  private void MovePieces()
  {
      if (_swipeAngle > -45 && _swipeAngle <= 45 && _colum < _gridElements.Width-1)
      {
         RightSwipe();
      }
      else if (_swipeAngle > 45 && _swipeAngle <= 135 && _row < _gridElements.Height-1)
      {
         UpSwipe();
      }
      else  if ((_swipeAngle > 135 || _swipeAngle <= -135) && _colum > 0)
      {
          LeftSwipe();
      }
      else if (_swipeAngle < -45 && _swipeAngle >= -135 && _row > 0)
      {
          DownSwipe();
      }

      StartCoroutine(ProcessMove());
  }

  private void DownSwipe()
  {
      _otherElement = _gridElements.GridElements[_colum, _row - 1];
      _previousRow = _row;// Задаем предыдущую строку равной текущей строке
      _previousColum = _colum;// Задаем предыдущую колонку равной текущей колонке
      _otherElement.GetComponent<ElementContoller>()._row += 1;
      _row -= 1;
  }
  private void LeftSwipe()
  {
      _otherElement = _gridElements.GridElements[_colum - 1, _row];
      _previousRow = _row;// Задаем предыдущую строку равной текущей строке
      _previousColum = _colum;// Задаем предыдущую колонку равной текущей колонке
      _otherElement.GetComponent<ElementContoller>()._colum += 1;
      _colum -= 1;
  }

  private void UpSwipe()
  {
      _otherElement = _gridElements.GridElements[_colum, _row +1];
      _previousRow = _row;// Задаем предыдущую строку равной текущей строке
      _previousColum = _colum;// Задаем предыдущую колонку равной текущей колонке
      _otherElement.GetComponent<ElementContoller>()._row -= 1;
      _row += 1;
  }
 private void RightSwipe()
 {
     _otherElement = _gridElements.GridElements[_colum + 1, _row];
         _previousRow = _row;// Задаем предыдущую строку равной текущей строке
         _previousColum = _colum;// Задаем предыдущую колонку равной текущей колонке
         _otherElement.GetComponent<ElementContoller>()._colum -= 1;
         _colum += 1;
     
 }
   //Метод FindMatches() проверяет наличие совпадений для фишки.
   //Если справа и слева от фишки есть две фишки с таким же тегом,
   //они помечаются как совпавшие (IsMatched = true).
   private void FindMatches()
   {
       if (_colum > 0 && _colum < _gridElements.Width - 1)
       {
           //Сначала метод проверяет, находится ли фишка внутри игрового поля.
           //Если да, то он получает ссылки на фишки, расположенные слева и справа от текущей фишки:
           GameObject leftElement = _gridElements.GridElements[_colum - 1, _row];
           GameObject rightElement= _gridElements.GridElements[_colum + 1, _row];
           CheckElements(leftElement,rightElement);
       }
       if (_row > 0 && _row < _gridElements.Height - 1)
       {
           GameObject upElement = _gridElements.GridElements[_colum, _row + 1];
           GameObject downElement = _gridElements.GridElements[_colum, _row - 1];
           CheckElements(downElement,upElement);
       }
   }

   private void CheckElements(GameObject negativeElement,GameObject positiveElement)
   {
       //метод сравнивает теги фишек слева и справа с тегом текущей фишки.
       //Если они совпадают, то обе фишки помечаются как совпавшие (IsMatched = true),
       //а также текущая фишка помечается как совпавшая (IsMatched = true):
       if  (positiveElement != null && negativeElement != null && positiveElement != gameObject && negativeElement != gameObject)
       {
           if (positiveElement.tag == gameObject.tag && negativeElement.tag == gameObject.tag)
           {
               positiveElement.GetComponent<ElementContoller>().IsMatched = true;
               negativeElement .GetComponent<ElementContoller>().IsMatched = true;
               IsMatched = true;
           }
       }
   }
   
}
