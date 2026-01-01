using System.Collections.Generic;
using UnityEngine;

namespace HB.UI
{
    /// <summary>
    /// Maps keyword strings to sprites under Resources/icons.
    /// (Doc: icons are loaded from icons/assets and shown near card frame.) fileciteturn7file1
    /// </summary>
    public class KeywordIconMapper : MonoBehaviour
    {
        [Header("Optional overrides")]
        public List<string> keywords = new List<string>();
        public List<Sprite> sprites = new List<Sprite>();

        private Dictionary<string, Sprite> _map;

        private void Awake()
        {
            _map = new Dictionary<string, Sprite>();
            for (int i = 0; i < Mathf.Min(keywords.Count, sprites.Count); i++)
                if (!string.IsNullOrWhiteSpace(keywords[i]) && sprites[i] != null)
                    _map[keywords[i].Trim()] = sprites[i];
        }

        public Sprite Resolve(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return null;

            if (_map != null && _map.TryGetValue(keyword.Trim(), out var s))
                return s;

            string k = keyword.Trim();
            string file = k switch
            {
                "Taunt" => "taunt",
                "Divine Shield" => "divine_shield",
                "DivineShield" => "divine_shield",
                "Reborn" => "reborn",
                "Windfury" => "windfury",
                _ => k.ToLower().Replace(" ", "_")
            };

            return Resources.Load<Sprite>($"icons/{file}");
        }
    }
}
