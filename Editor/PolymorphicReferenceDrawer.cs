#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CupkekGames.EditorUI
{
    /// <summary>
    /// Reusable property drawer for <c>[SerializeReference]</c> polymorphic
    /// fields and lists. Renders a clickable button showing the current
    /// concrete type; clicking opens a <see cref="SearchableDropdown"/> of
    /// concrete subclasses of <typeparamref name="TBase"/>. Once a type is
    /// chosen, the chosen instance's serialised fields render below.
    ///
    /// Subclass with <c>[CustomPropertyDrawer(typeof(MyBase), true)]</c>:
    /// <code>
    /// [CustomPropertyDrawer(typeof(IFeature), true)]
    /// public class FeatureDrawer : PolymorphicReferenceDrawer&lt;IFeature&gt;
    /// {
    ///     protected override string GetGroup(Type t) =&gt;
    ///         t.GetCustomAttribute&lt;FeatureGroupAttribute&gt;()?.Group;
    /// }
    /// </code>
    ///
    /// Default behaviour:
    /// <list type="bullet">
    ///   <item>Type filter: every concrete (non-abstract, non-interface,
    ///     non-open-generic) subclass of the resolved element constraint
    ///     — list field → element type; plain field → field type if it's
    ///     assignable to <typeparamref name="TBase"/>.</item>
    ///   <item>Display label: <c>Type.Name</c>.</item>
    ///   <item>Group: none (override <see cref="GetGroup"/> for categorised
    ///     dropdowns).</item>
    ///   <item>Empty-state label: <c>"(none — click to choose)"</c>.</item>
    /// </list>
    /// </summary>
    public abstract class PolymorphicReferenceDrawer<TBase> : PropertyDrawer
    {
        // ---------------------------------------------------------------
        // Configuration hooks
        // ---------------------------------------------------------------

        /// <summary>
        /// The element-type filter used when populating the dropdown.
        /// Default: for <c>List&lt;TX&gt;</c> where <c>TX : TBase</c>, returns
        /// <c>TX</c>; for plain <c>TX : TBase</c> fields returns <c>TX</c>;
        /// otherwise returns <c>typeof(TBase)</c>. Subclasses override only
        /// when they need narrower filtering than the field signature gives.
        /// </summary>
        protected virtual Type ResolveElementConstraint()
        {
            if (fieldInfo == null) return typeof(TBase);
            Type fieldType = fieldInfo.FieldType;

            if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type element = fieldType.GetGenericArguments()[0];
                if (typeof(TBase).IsAssignableFrom(element)) return element;
            }
            if (typeof(TBase).IsAssignableFrom(fieldType)) return fieldType;
            return typeof(TBase);
        }

        /// <summary>Display label shown in the dropdown and on the type button. Default: <c>type.Name</c>.</summary>
        protected virtual string GetDisplayName(Type type) => type.Name;

        /// <summary>Optional group / category for the dropdown. Default: none.</summary>
        protected virtual string GetGroup(Type type) => null;

        /// <summary>Empty-state label shown when no concrete type has been picked yet.</summary>
        protected virtual string EmptyLabel => "(none — click to choose)";

        // ---------------------------------------------------------------
        // IMGUI
        // ---------------------------------------------------------------

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float line = EditorGUIUtility.singleLineHeight;
            float sp = EditorGUIUtility.standardVerticalSpacing;
            return line + sp + EditorImGuiDrawing.GetChildPropertiesHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float line = EditorGUIUtility.singleLineHeight;
            float sp = EditorGUIUtility.standardVerticalSpacing;
            string typeName = GetCurrentLabel(property);
            Rect btnRect = new Rect(position.x, position.y, position.width, line);

            if (EditorGUI.DropdownButton(btnRect, new GUIContent($"{label.text}: {typeName}"), FocusType.Keyboard))
            {
                var items = BuildDropdownItems();
                var screenRect = new Rect(
                    GUIUtility.GUIToScreenPoint(new Vector2(btnRect.x, btnRect.yMax)),
                    new Vector2(btnRect.width, 1));

                SearchableDropdown.Show(screenRect, items, selectedKey =>
                {
                    var type = Type.GetType(selectedKey);
                    if (type == null) return;
                    property.serializedObject.Update();
                    property.managedReferenceValue = Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                }, property.managedReferenceValue?.GetType().AssemblyQualifiedName);
            }

            float y = position.y + line + sp;
            EditorGUI.indentLevel++;
            EditorImGuiDrawing.DrawChildProperties(position, property, ref y);
            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        // ---------------------------------------------------------------
        // UI Toolkit
        // ---------------------------------------------------------------

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var childContainer = new VisualElement
            {
                style = { paddingLeft = 12f },
            };

            var typeButton = BuildTypeButton(property, childContainer);
            container.Add(typeButton);
            container.Add(childContainer);

            RebuildChildren(childContainer, property);
            return container;
        }

        VisualElement BuildTypeButton(SerializedProperty property, VisualElement childContainer)
        {
            var button = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    paddingTop = 2f,
                    paddingBottom = 2f,
                    paddingLeft = 6f,
                    paddingRight = 4f,
                    borderTopWidth = 1f,
                    borderBottomWidth = 1f,
                    borderLeftWidth = 1f,
                    borderRightWidth = 1f,
                    borderTopColor = new Color(0.2f, 0.2f, 0.2f, 0.6f),
                    borderBottomColor = new Color(0.2f, 0.2f, 0.2f, 0.6f),
                    borderLeftColor = new Color(0.2f, 0.2f, 0.2f, 0.6f),
                    borderRightColor = new Color(0.2f, 0.2f, 0.2f, 0.6f),
                    borderTopLeftRadius = 3f,
                    borderTopRightRadius = 3f,
                    borderBottomLeftRadius = 3f,
                    borderBottomRightRadius = 3f,
                    backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.4f),
                },
            };

            var label = new Label(GetCurrentLabel(property))
            {
                style =
                {
                    flexGrow = 1,
                    unityTextAlign = TextAnchor.MiddleLeft,
                },
            };
            button.Add(label);

            var arrow = new Label("▼")
            {
                style =
                {
                    fontSize = 8,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    width = 14,
                    color = new Color(0.7f, 0.7f, 0.7f),
                },
            };
            button.Add(arrow);

            button.RegisterCallback<ClickEvent>(_ =>
            {
                var items = BuildDropdownItems();
                SearchableDropdown.Show(button, items, selectedKey =>
                {
                    var type = Type.GetType(selectedKey);
                    if (type == null) return;
                    property.serializedObject.Update();
                    property.managedReferenceValue = Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                    label.text = GetDisplayName(type);
                    RebuildChildren(childContainer, property);
                }, property.managedReferenceValue?.GetType().AssemblyQualifiedName);
            });

            return button;
        }

        // ---------------------------------------------------------------
        // Helpers
        // ---------------------------------------------------------------

        string GetCurrentLabel(SerializedProperty property)
        {
            var current = property.managedReferenceValue?.GetType();
            return current != null ? GetDisplayName(current) : EmptyLabel;
        }

        List<SearchableDropdown.DropdownItem> BuildDropdownItems()
        {
            var items = new List<SearchableDropdown.DropdownItem>();
            Type constraint = ResolveElementConstraint();

            foreach (var t in TypeCache.GetTypesDerivedFrom(constraint))
            {
                if (t.IsAbstract || t.IsInterface || t.IsGenericTypeDefinition) continue;
                items.Add(new SearchableDropdown.DropdownItem
                {
                    Key = t.AssemblyQualifiedName,
                    DisplayName = GetDisplayName(t),
                    Group = GetGroup(t),
                });
            }

            items.Sort((a, b) =>
            {
                int g = string.Compare(a.Group ?? "", b.Group ?? "", StringComparison.Ordinal);
                return g != 0 ? g : string.Compare(a.DisplayName, b.DisplayName, StringComparison.Ordinal);
            });
            return items;
        }

        static void RebuildChildren(VisualElement childContainer, SerializedProperty property)
        {
            childContainer.Clear();
            if (property.managedReferenceValue == null) return;

            var iter = property.Copy();
            var end = property.GetEndProperty();
            bool enterChildren = true;

            while (iter.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iter, end))
            {
                enterChildren = false;
                childContainer.Add(new PropertyField(iter));
            }

            childContainer.Bind(property.serializedObject);
        }
    }
}
#endif
