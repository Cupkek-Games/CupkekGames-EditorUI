#if UNITY_EDITOR
using UnityEngine;

namespace CupkekGames.EditorUI
{
    /// <summary>
    /// Solid (opaque) editor colors for badges, alerts, and base surfaces.
    /// Pair this with <see cref="EditorColorPalette"/> which holds alpha-blended overlay
    /// colors for borders/surfaces drawn on top of arbitrary backgrounds.
    ///
    /// Use this palette when you want a flat solid colored background (badge, status pill,
    /// alert box). Use <see cref="EditorColorPalette"/> when you want a subtle highlight
    /// that respects whatever the underlying editor theme is.
    /// </summary>
    public static class EditorSolidColors
    {
        // ─── Status (semantic, opaque) ─────────────────────────────────────────
        public static readonly Color Primary = new Color(0.35f, 0.45f, 0.55f, 1f);
        public static readonly Color PrimaryContent = new Color(0.9f, 0.9f, 0.9f, 1f);

        public static readonly Color Secondary = new Color(0.4f, 0.4f, 0.45f, 1f);
        public static readonly Color SecondaryContent = new Color(0.9f, 0.9f, 0.9f, 1f);

        public static readonly Color Success = new Color(0.35f, 0.5f, 0.4f, 1f);
        public static readonly Color SuccessContent = new Color(0.9f, 0.9f, 0.9f, 1f);

        public static readonly Color Warning = new Color(0.6f, 0.5f, 0.3f, 1f);
        public static readonly Color WarningContent = new Color(0.95f, 0.95f, 0.95f, 1f);

        public static readonly Color Error = new Color(0.55f, 0.3f, 0.3f, 1f);
        public static readonly Color ErrorContent = new Color(0.95f, 0.95f, 0.95f, 1f);

        public static readonly Color Info = new Color(0.35f, 0.45f, 0.55f, 1f);
        public static readonly Color InfoContent = new Color(0.9f, 0.9f, 0.9f, 1f);

        public static readonly Color Neutral = new Color(0.35f, 0.35f, 0.35f, 1f);
        public static readonly Color NeutralContent = new Color(0.85f, 0.85f, 0.85f, 1f);

        // ─── Base (background scales) ─────────────────────────────────────────
        public static readonly Color Base100 = new Color(0.22f, 0.22f, 0.22f, 1f);
        public static readonly Color Base200 = new Color(0.18f, 0.18f, 0.18f, 1f);
        public static readonly Color Base300 = new Color(0.14f, 0.14f, 0.14f, 1f);
        public static readonly Color BaseContent = new Color(0.85f, 0.85f, 0.85f, 1f);
        public static readonly Color TextMuted = new Color(0.6f, 0.6f, 0.6f, 1f);

        public static readonly Color Border = new Color(0.2f, 0.2f, 0.2f, 1f);
    }
}
#endif
