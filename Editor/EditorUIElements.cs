#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CupkekGames.EditorUI
{
    /// <summary>
    /// Factory methods for creating styled UI Toolkit elements (badges, alerts, status dots,
    /// boxes, dividers, buttons, labels, layout helpers). Uses <see cref="EditorSolidColors"/>
    /// for all coloring; for alpha-blend overlay colors see <see cref="EditorColorPalette"/>.
    /// </summary>
    public static class EditorUIElements
    {
        #region Badge

        public enum BadgeStyle
        {
            Primary,
            Secondary,
            Success,
            Warning,
            Error,
            Info,
            Neutral
        }

        /// <summary>
        /// Creates a badge (pill-shaped label).
        /// </summary>
        public static VisualElement CreateBadge(string text, BadgeStyle style = BadgeStyle.Neutral)
        {
            var (bgColor, textColor) = GetBadgeColors(style);
            return CreateBadge(text, bgColor, textColor);
        }

        /// <summary>
        /// Creates a badge with custom colors.
        /// </summary>
        public static VisualElement CreateBadge(string text, Color backgroundColor, Color textColor)
        {
            var badge = new VisualElement();
            badge.style.flexDirection = FlexDirection.Row;
            badge.style.alignItems = Align.Center;
            badge.style.backgroundColor = backgroundColor;
            badge.style.borderTopLeftRadius = 8f;
            badge.style.borderTopRightRadius = 8f;
            badge.style.borderBottomLeftRadius = 8f;
            badge.style.borderBottomRightRadius = 8f;
            badge.style.paddingLeft = 8f;
            badge.style.paddingRight = 8f;
            badge.style.paddingTop = 2f;
            badge.style.paddingBottom = 2f;
            badge.style.marginRight = 4f;
            badge.style.marginBottom = 2f;

            var label = new Label(text);
            label.style.fontSize = 11f;
            label.style.color = textColor;
            badge.Add(label);

            return badge;
        }

        /// <summary>
        /// Creates a removable badge with an X button.
        /// </summary>
        public static VisualElement CreateRemovableBadge(string text, Action onRemove, BadgeStyle style = BadgeStyle.Info)
        {
            var (bgColor, textColor) = GetBadgeColors(style);
            return CreateRemovableBadge(text, onRemove, bgColor, textColor);
        }

        /// <summary>
        /// Creates a removable badge with custom colors.
        /// </summary>
        public static VisualElement CreateRemovableBadge(string text, Action onRemove, Color backgroundColor, Color textColor)
        {
            var badge = new VisualElement();
            badge.style.flexDirection = FlexDirection.Row;
            badge.style.alignItems = Align.Center;
            badge.style.backgroundColor = backgroundColor;
            badge.style.borderTopLeftRadius = 8f;
            badge.style.borderTopRightRadius = 8f;
            badge.style.borderBottomLeftRadius = 8f;
            badge.style.borderBottomRightRadius = 8f;
            badge.style.paddingLeft = 6f;
            badge.style.paddingRight = 4f;
            badge.style.paddingTop = 2f;
            badge.style.paddingBottom = 2f;
            badge.style.marginRight = 4f;
            badge.style.marginBottom = 2f;

            var label = new Label(text);
            label.style.fontSize = 11f;
            label.style.color = textColor;
            badge.Add(label);

            var removeBtn = CreateIconButton("×", EditorSolidColors.Error, () => onRemove?.Invoke());
            removeBtn.style.width = 16f;
            removeBtn.style.height = 16f;
            removeBtn.style.marginLeft = 4f;
            badge.Add(removeBtn);

            return badge;
        }

        private static (Color bg, Color text) GetBadgeColors(BadgeStyle style)
        {
            return style switch
            {
                BadgeStyle.Primary => (EditorSolidColors.Primary, EditorSolidColors.PrimaryContent),
                BadgeStyle.Secondary => (EditorSolidColors.Secondary, EditorSolidColors.SecondaryContent),
                BadgeStyle.Success => (EditorSolidColors.Success, EditorSolidColors.SuccessContent),
                BadgeStyle.Warning => (EditorSolidColors.Warning, EditorSolidColors.WarningContent),
                BadgeStyle.Error => (EditorSolidColors.Error, EditorSolidColors.ErrorContent),
                BadgeStyle.Info => (EditorSolidColors.Info, EditorSolidColors.InfoContent),
                _ => (EditorSolidColors.Neutral, EditorSolidColors.NeutralContent)
            };
        }

        #endregion

        #region Status

        public enum StatusType
        {
            Success,
            Warning,
            Error,
            Info,
            Neutral,
            Online,
            Offline
        }

        /// <summary>
        /// Creates a small status dot indicator.
        /// </summary>
        public static VisualElement CreateStatusDot(StatusType status, float size = 8f)
        {
            var dot = new VisualElement();
            dot.style.width = size;
            dot.style.height = size;
            dot.style.borderTopLeftRadius = size / 2f;
            dot.style.borderTopRightRadius = size / 2f;
            dot.style.borderBottomLeftRadius = size / 2f;
            dot.style.borderBottomRightRadius = size / 2f;
            dot.style.backgroundColor = GetStatusColor(status);
            return dot;
        }

        /// <summary>
        /// Creates a status indicator with text.
        /// </summary>
        public static VisualElement CreateStatus(string text, StatusType status)
        {
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;
            container.style.alignItems = Align.Center;

            var dot = CreateStatusDot(status);
            dot.style.marginRight = 4f;
            container.Add(dot);

            var label = new Label(text);
            label.style.fontSize = 11f;
            container.Add(label);

            return container;
        }

        private static Color GetStatusColor(StatusType status)
        {
            return status switch
            {
                StatusType.Success or StatusType.Online => EditorSolidColors.Success,
                StatusType.Warning => EditorSolidColors.Warning,
                StatusType.Error or StatusType.Offline => EditorSolidColors.Error,
                StatusType.Info => EditorSolidColors.Info,
                _ => EditorSolidColors.Neutral
            };
        }

        #endregion

        #region Box

        /// <summary>
        /// Creates a styled container box.
        /// </summary>
        public static VisualElement CreateBox(Color? backgroundColor = null, float borderRadius = 4f)
        {
            var box = new VisualElement();
            box.style.backgroundColor = backgroundColor ?? EditorSolidColors.Base100;
            box.style.borderTopLeftRadius = borderRadius;
            box.style.borderTopRightRadius = borderRadius;
            box.style.borderBottomLeftRadius = borderRadius;
            box.style.borderBottomRightRadius = borderRadius;
            box.style.borderTopWidth = 1f;
            box.style.borderBottomWidth = 1f;
            box.style.borderLeftWidth = 1f;
            box.style.borderRightWidth = 1f;
            box.style.borderTopColor = EditorSolidColors.Border;
            box.style.borderBottomColor = EditorSolidColors.Border;
            box.style.borderLeftColor = EditorSolidColors.Border;
            box.style.borderRightColor = EditorSolidColors.Border;
            box.style.paddingTop = 8f;
            box.style.paddingBottom = 8f;
            box.style.paddingLeft = 8f;
            box.style.paddingRight = 8f;
            return box;
        }

        /// <summary>
        /// Creates an alert box with icon and message.
        /// </summary>
        public static VisualElement CreateAlert(string message, StatusType type)
        {
            var (bgColor, textColor) = type switch
            {
                StatusType.Success => (new Color(0.25f, 0.3f, 0.25f, 0.5f), EditorSolidColors.BaseContent),
                StatusType.Warning => (new Color(0.35f, 0.3f, 0.2f, 0.5f), EditorSolidColors.BaseContent),
                StatusType.Error => (new Color(0.35f, 0.2f, 0.2f, 0.5f), EditorSolidColors.BaseContent),
                StatusType.Info => (new Color(0.25f, 0.28f, 0.32f, 0.5f), EditorSolidColors.BaseContent),
                _ => (EditorSolidColors.Base200, EditorSolidColors.BaseContent)
            };

            var alert = CreateBox(bgColor);
            alert.style.flexDirection = FlexDirection.Row;
            alert.style.alignItems = Align.Center;

            var dot = CreateStatusDot(type, 10f);
            dot.style.marginRight = 8f;
            alert.Add(dot);

            var label = new Label(message);
            label.style.color = textColor;
            label.style.whiteSpace = WhiteSpace.Normal;
            label.style.flexGrow = 1;
            alert.Add(label);

            return alert;
        }

        /// <summary>
        /// Creates a horizontal divider line.
        /// </summary>
        public static VisualElement CreateDivider(float padding = 8f)
        {
            var container = new VisualElement();
            container.style.paddingTop = padding;
            container.style.paddingBottom = padding;

            var line = new VisualElement();
            line.style.height = 1f;
            line.style.backgroundColor = EditorSolidColors.Border;
            container.Add(line);

            return container;
        }

        #endregion

        #region Buttons

        /// <summary>
        /// Creates a standard Unity-style button (default look).
        /// </summary>
        public static Button CreateButton(string text, Action onClick)
        {
            var btn = new Button { text = text };
            btn.clicked += () => onClick?.Invoke();
            return btn;
        }

        /// <summary>
        /// Creates an outline-style primary button (subtle accent).
        /// </summary>
        public static Button CreatePrimaryButton(string text, Action onClick)
        {
            var btn = new Button { text = text };
            // Keep Unity's default background, just add accent border
            btn.style.borderTopWidth = 1f;
            btn.style.borderBottomWidth = 1f;
            btn.style.borderLeftWidth = 1f;
            btn.style.borderRightWidth = 1f;
            btn.style.borderTopColor = new Color(0.4f, 0.5f, 0.6f, 1f);
            btn.style.borderBottomColor = new Color(0.4f, 0.5f, 0.6f, 1f);
            btn.style.borderLeftColor = new Color(0.4f, 0.5f, 0.6f, 1f);
            btn.style.borderRightColor = new Color(0.4f, 0.5f, 0.6f, 1f);
            btn.clicked += () => onClick?.Invoke();
            return btn;
        }

        /// <summary>
        /// Creates an outline-style danger button (subtle warning).
        /// </summary>
        public static Button CreateDangerButton(string text, Action onClick)
        {
            var btn = new Button { text = text };
            // Keep Unity's default background, just add red-ish border
            btn.style.borderTopWidth = 1f;
            btn.style.borderBottomWidth = 1f;
            btn.style.borderLeftWidth = 1f;
            btn.style.borderRightWidth = 1f;
            btn.style.borderTopColor = new Color(0.5f, 0.35f, 0.35f, 1f);
            btn.style.borderBottomColor = new Color(0.5f, 0.35f, 0.35f, 1f);
            btn.style.borderLeftColor = new Color(0.5f, 0.35f, 0.35f, 1f);
            btn.style.borderRightColor = new Color(0.5f, 0.35f, 0.35f, 1f);
            btn.clicked += () => onClick?.Invoke();
            return btn;
        }

        /// <summary>
        /// Creates a small icon button (like × or +). Uses Unity default style.
        /// </summary>
        public static Button CreateIconButton(string icon, Color? borderColor = null, Action onClick = null)
        {
            var btn = new Button { text = icon };
            btn.style.width = 20f;
            btn.style.height = 20f;
            btn.style.paddingLeft = 0f;
            btn.style.paddingRight = 0f;
            btn.style.paddingTop = 0f;
            btn.style.paddingBottom = 0f;
            btn.style.minWidth = 20f;

            // Add subtle border accent if color provided
            if (borderColor.HasValue)
            {
                btn.style.borderTopWidth = 1f;
                btn.style.borderBottomWidth = 1f;
                btn.style.borderLeftWidth = 1f;
                btn.style.borderRightWidth = 1f;
                btn.style.borderTopColor = borderColor.Value;
                btn.style.borderBottomColor = borderColor.Value;
                btn.style.borderLeftColor = borderColor.Value;
                btn.style.borderRightColor = borderColor.Value;
            }

            if (onClick != null)
                btn.clicked += () => onClick();

            return btn;
        }

        /// <summary>
        /// Creates a text-only link-style button.
        /// </summary>
        public static Button CreateLinkButton(string text, Action onClick)
        {
            var btn = new Button { text = text };
            btn.style.backgroundColor = Color.clear;
            btn.style.color = new Color(0.5f, 0.6f, 0.7f, 1f);
            btn.style.borderTopWidth = 0f;
            btn.style.borderBottomWidth = 0f;
            btn.style.borderLeftWidth = 0f;
            btn.style.borderRightWidth = 0f;
            btn.style.paddingLeft = 0f;
            btn.style.paddingRight = 0f;
            btn.clicked += () => onClick?.Invoke();
            return btn;
        }

        #endregion

        #region Labels

        /// <summary>
        /// Creates a hint/placeholder style label.
        /// </summary>
        public static Label CreateHintLabel(string text)
        {
            var label = new Label(text);
            label.style.color = new Color(0.6f, 0.6f, 0.6f);
            label.style.unityFontStyleAndWeight = FontStyle.Italic;
            return label;
        }

        /// <summary>
        /// Creates a bold header label.
        /// </summary>
        public static Label CreateHeaderLabel(string text)
        {
            var label = new Label(text);
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            return label;
        }

        #endregion

        #region Layout

        /// <summary>
        /// Creates a horizontal container with flexbox row layout.
        /// </summary>
        public static VisualElement CreateRow(Align alignItems = Align.Center, Justify justifyContent = Justify.FlexStart)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = alignItems;
            row.style.justifyContent = justifyContent;
            return row;
        }

        /// <summary>
        /// Creates a wrapping horizontal container for badges/tags.
        /// </summary>
        public static VisualElement CreateWrapRow()
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.flexWrap = Wrap.Wrap;
            row.style.alignItems = Align.Center;
            return row;
        }

        /// <summary>
        /// Creates an inspector property row with a label (1x) and input container (2x).
        /// Returns (row, label, inputContainer) so callers can add elements to the input side.
        /// </summary>
        public static (VisualElement row, Label label, VisualElement input) CreatePropertyRow(string labelText)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.flexGrow = 1;

            var label = new Label(labelText);
            label.style.flexGrow = 1;
            label.style.flexBasis = 0;
            label.style.minWidth = 0;
            label.style.overflow = Overflow.Hidden;
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            row.Add(label);

            var input = new VisualElement();
            input.style.flexDirection = FlexDirection.Row;
            input.style.flexGrow = 4;
            input.style.flexBasis = 0;
            input.style.flexShrink = 1;
            input.style.minWidth = 0;
            row.Add(input);

            return (row, label, input);
        }

        #endregion
    }
}
#endif
