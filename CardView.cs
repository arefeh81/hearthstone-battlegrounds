using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HB.UI
{
    /// <summary>
    /// Visual + drag source for a card (minion). Attach to Card prefab.
    /// </summary>
    public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [Header("UI")]
        public TMP_Text nameText;
        public TMP_Text statsText;
        public Transform keywordsRoot;
        public Image keywordIconPrefab;

        [Header("Config")]
        public CanvasGroup canvasGroup;

        public CardDTO Data { get; private set; }
        public ZoneRef Source { get; private set; }

        private KeywordIconMapper _iconMapper;

        public event Action<CardView> Clicked;

        public void Bind(CardDTO dto, ZoneRef source, KeywordIconMapper mapper)
        {
            Data = dto;
            Source = source;
            _iconMapper = mapper;

            if (nameText) nameText.text = dto.name;
            if (statsText) statsText.text = $"{dto.attack}/{dto.health}  ★{dto.tier}" + (dto.golden ? " (Golden)" : "");
            RenderKeywords(dto.keywords);
        }

        private void RenderKeywords(List<string> keywords)
        {
            if (!keywordsRoot || !keywordIconPrefab) return;

            for (int i = keywordsRoot.childCount - 1; i >= 0; i--)
                Destroy(keywordsRoot.GetChild(i).gameObject);

            if (keywords == null) return;
            foreach (var k in keywords)
            {
                var spr = _iconMapper ? _iconMapper.Resolve(k) : null;
                if (!spr) continue;

                var icon = Instantiate(keywordIconPrefab, keywordsRoot);
                icon.sprite = spr;
                icon.gameObject.SetActive(true);
            }
        }

        public void OnPointerClick(PointerEventData eventData) => Clicked?.Invoke(this);

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragService.Instance?.BeginDrag(this, eventData);
            if (canvasGroup) canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData) => DragService.Instance?.OnDrag(eventData);

        public void OnEndDrag(PointerEventData eventData)
        {
            DragService.Instance?.EndDrag(eventData);
            if (canvasGroup) canvasGroup.blocksRaycasts = true;
        }
    }
}
