using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// ��¥ : 2021-07-28 AM 2:05:41
// �ۼ��� : Rito

namespace Rito
{
    /// <summary> 
    /// ���� Ⱦ��ũ�� UI
    /// </summary>
    public class InfiniteHorizontalScroll : MonoBehaviour
    {
        /***********************************************************************
        *                           Inspector Fields
        ***********************************************************************/
        #region .
        [SerializeField] private Vector2 centerUISize = new Vector2(200f, 200f);  // �߾� UI�� ũ��
        [SerializeField] private Vector2 edgeUISize = new Vector2(100f, 100f);    // ���� �ܰ� UI�� ũ��

        [Range(0f, 100f)]
        [SerializeField] private float spaceWidth = 25f; // �̹��� ���� ����

        [Range(0.01f, 2f)]
        [SerializeField] private float transitionTime = 0.5f; // ��ȯ�� �ɸ��� �ð�

        [SerializeField] private bool useImposter = true; // �������� ��� ����

        #endregion
        /***********************************************************************
        *                           Private Fields
        ***********************************************************************/
        #region .
        private const int LEFT = 1;
        private const int RIGHT = -1;

        private List<RectTransform> _targetList = new List<RectTransform>(8);
        private List<RectTransform> _imposterList = new List<RectTransform>(8);
        private int _targetCount;

        private RectTransform _currentImposter;

        [SerializeField]
        private int _currentIndex = 0;

        private bool _isTransiting = false;
        private float _progress; // ���൵ : 0 ~ 1
        private int _direction;

        #endregion
        /***********************************************************************
        *                           Look Up Tables
        ***********************************************************************/
        #region .
        private int _lookupCount;
        private int _lookupCenterIndex;

        private float[] _xPosTable; // �ε��� ��ġ�� ���� X ��ǥ ���
        private Vector2[] _sizeTable; // �ε��� ��ġ�� ���� ũ�� ���

        private void GenerateLookUpTables()
        {
            _xPosTable = new float[_lookupCount];
            _sizeTable = new Vector2[_lookupCount];

            for (int i = 0; i < _lookupCount; i++)
            {
                _xPosTable[i] = GetXPosition(i);
                _sizeTable[i] = GetSize(i);
            }
        }

        /// <summary> lookup center index���� �ε��� ���� ��� </summary>
        private int GetIndexDiffFromCenter(int index)
        {
            return Mathf.Abs(index - _lookupCenterIndex);
        }

        /// <summary> ���μ��� ũ�� ���ϱ� </summary>
        private Vector2 GetSize(int lookupIndex)
        {
            if (lookupIndex == _lookupCenterIndex)
                return centerUISize;

            float absGap = GetIndexDiffFromCenter(lookupIndex);

            return Vector2.Lerp(centerUISize, edgeUISize, absGap / _lookupCenterIndex);
        }

        /// <summary> X��ǥ ���ϱ� </summary>
        private float GetXPosition(int lookupIndex)
        {
            // �߾� ��ġ
            if (lookupIndex == _lookupCenterIndex)
                return 0f;

            float absGap = GetIndexDiffFromCenter(lookupIndex);

            // // 1. ����� ��
            float pos = absGap * spaceWidth;

            // 2. �ʺ� ��
            for (int i = 0; i <= absGap; i++)
            {
                float w = Vector2.Lerp(centerUISize, edgeUISize, i / (float)_lookupCenterIndex).x;

                if (0 < i && i < absGap)
                    pos += w;
                else
                    pos += w * 0.5f;
            }

            return (lookupIndex < _lookupCenterIndex) ? -pos : pos;
        }

        #endregion
        /***********************************************************************
        *                           Unity Events
        ***********************************************************************/
        #region .
        private void OnEnable()
        {
            _targetList.Clear();
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                GameObject go = child.gameObject;

                if (go.activeSelf == false) continue;
                if (go.hideFlags == HideFlags.HideInHierarchy) continue;

                _targetList.Add(child.GetComponent<RectTransform>());
            }

            _targetCount = _targetList.Count;
            _lookupCount = _targetCount + 2;
            _lookupCenterIndex = _lookupCount / 2;

            GenerateLookUpTables();
            InitRectTransforms();

            if (useImposter) GenerateImposters();
        }

        private void Update()
        {
            if (!_isTransiting)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    _direction = LEFT;
                    Local_BeginTransition();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    _direction = RIGHT;
                    Local_BeginTransition();
                }
            }
            else
            {
                _progress += Time.deltaTime / transitionTime;
                if (_progress > 1f) _progress = 1f;

                OnTransition();

                if (_progress == 1f)
                {
                    _isTransiting = false;
                    _currentIndex = (_currentIndex - _direction) % _targetCount;
                    if (_currentIndex < 0) _currentIndex += _targetCount;
                    OnTransitionEnd();
                }
            }

            void Local_BeginTransition()
            {
                _isTransiting = true;
                _progress = 0;
                OnTransitionBegin();
            }
        }

        private void OnTransitionBegin()
        {
            if (useImposter) InitImposter();
        }

        private void OnTransition()
        {
            MoveAll();
            if (useImposter) MoveImposter();
        }

        private void OnTransitionEnd()
        {
            for (int i = 0; i < _targetCount; i++)
            {
                int li = GetLookupIndex(i);

                // ������ �̹����� �˸��� ��ġ�� �̵�
                if (li == 1 || li == _targetCount)
                {
                    _targetList[i].SetSizeAndXPosition(_sizeTable[li], _xPosTable[li]);
                }
            }

            // �������� ��Ȱ��ȭ
            if (useImposter)
                _currentImposter.gameObject.SetActive(false);
        }
        #endregion
        /***********************************************************************
        *                           Getter Methods
        ***********************************************************************/
        #region .
        /// <summary> index�� lookup index�� �����ϱ� </summary>
        private int GetLookupIndex(int index)
        {
            index = index - _currentIndex + _lookupCenterIndex;

            if (index > _targetCount)
                index -= _targetCount;

            else if (index <= 0)
                index += _targetCount;

            return index;
        }

        /// <summary> lookup index�� index�� �����ϱ� </summary>
        private int GetIndexFromLookupIndex(int lookupIndex)
        {
            int index = lookupIndex - _lookupCenterIndex + _currentIndex;

            if (index < 0)
                index += _targetCount;

            return index % _targetCount;
        }

        /// <summary> ���� ���� �̹����� ���� �ε��� </summary>
        private int GetLeftImageIndex() => GetIndexFromLookupIndex(0);
        /// <summary> ���� ���� �̹����� ���� �ε��� </summary>
        private int GetRightImageIndex() => GetIndexFromLookupIndex(_lookupCount - 1);

        #endregion
        /***********************************************************************
        *                           Rect Transform Methods
        ***********************************************************************/
        #region .
        /// <summary> Rect Transform X Pos, Size �ʱ� ���� </summary>
        private void InitRectTransforms()
        {
            for (int i = 0; i < _targetCount; i++)
            {
                int li = GetLookupIndex(i);
                float xPos = _xPosTable[li];
                Vector2 size = _sizeTable[li];

                _targetList[i].SetSizeAndXPosition(size, xPos);
            }
        }

        /// <summary> ��� �̹����� Rect Transform �̵���Ű�� </summary>
        private void MoveAll()
        {
            for (int i = 0; i < _targetCount; i++)
            {
                int li = GetLookupIndex(i);

                float curXPos = _xPosTable[li];
                float nextXPos = _xPosTable[li + _direction];
                float xPos = Mathf.Lerp(curXPos, nextXPos, _progress);

                Vector2 curSize = _sizeTable[li];
                Vector2 nextSize = _sizeTable[li + _direction];
                Vector2 size = Vector2.Lerp(curSize, nextSize, _progress);
                _targetList[i].SetSizeAndXPosition(size, xPos);
            }
        }
        #endregion
        /***********************************************************************
        *                           Imposter Methods
        ***********************************************************************/
        #region .
        private void GenerateImposters()
        {
            _imposterList.Clear();

            for (int i = 0; i < _targetCount; i++)
            {
                GameObject go = Instantiate(_targetList[i].gameObject);
                go.transform.SetParent(transform);
                go.hideFlags = HideFlags.HideInHierarchy;

                RectTransform rt = go.GetComponent<RectTransform>();
                _imposterList.Add(rt);
                go.SetActive(false);
            }
        }

        private void InitImposter()
        {
            int index, lookupIndex;
            float xPos;
            Vector2 size;

            switch (_direction)
            {
                default:
                case LEFT:
                    index = GetLeftImageIndex();
                    lookupIndex = 0;
                    break;

                case RIGHT:
                    index = GetRightImageIndex();
                    lookupIndex = _lookupCount - 1;
                    break;
            }

            xPos = _xPosTable[lookupIndex];
            size = _sizeTable[lookupIndex];

            _currentImposter = _imposterList[index];
            _currentImposter.SetSizeAndXPosition(size, xPos);
            _currentImposter.gameObject.SetActive(true);
        }

        private void MoveImposter()
        {
            int lookupIndex, lookupIndexNext;

            switch (_direction)
            {
                default:
                case LEFT:
                    lookupIndex = 0;
                    lookupIndexNext = 1;
                    break;

                case RIGHT:
                    lookupIndex = _lookupCount - 1;
                    lookupIndexNext = _lookupCount - 2;
                    break;
            }

            float curXPos = _xPosTable[lookupIndex];
            float nextXPos = _xPosTable[lookupIndexNext];
            float xPos = Mathf.Lerp(curXPos, nextXPos, _progress);

            Vector2 curSize = _sizeTable[lookupIndex];
            Vector2 nextSize = _sizeTable[lookupIndexNext];
            Vector2 size = Vector2.Lerp(curSize, nextSize, _progress);

            _currentImposter.SetSizeAndXPosition(size, xPos);
        }
        #endregion
        /***********************************************************************
        *                           Public Methods
        ***********************************************************************/
        #region .
        /// <summary> ���� ���õ� UI�� �ε��� ���� </summary>
        public int GetCurrentIndex() => _currentIndex;
        #endregion
    }

    /***********************************************************************
    *                               Extension Helpers
    ***********************************************************************/
    #region .
    static class RectTransformExtensionHelper
    {
        public static void SetSizeAndXPosition(this RectTransform @this, in Vector2 size, float xPos)
        {
            @this.sizeDelta = size;
            @this.anchoredPosition = new Vector2(xPos, 0f);
        }
    }

    #endregion
}