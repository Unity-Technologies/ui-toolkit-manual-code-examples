using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class AspectRatio : VisualElement
	{
		// The ratio of width.
		[UxmlAttribute("width")]
		public int RatioWidth { get; private set; } = 16;

		// The ratio of height.

		[UxmlAttribute("height")]
		public int RatioHeight { get; private set; } = 9;

		// Padding elements to keep the aspect ratio.
		private VisualElement leftPadding;
		private VisualElement rightPadding;

		public AspectRatio()
		{
			style.flexDirection = FlexDirection.Row;
			style.flexShrink = 0;
			style.width = Length.Percent(100);
			style.height = Length.Percent(100);

			leftPadding = new VisualElement() { name = "AspectRatio-Left" };
			rightPadding = new VisualElement() { name = "AspectRatio-Right" };

			Add(leftPadding);
			Add(rightPadding);

			RegisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanelEvent);
		}

		// Update the padding elements when the geometry changes.
		private void OnGeometryChangedEvent(GeometryChangedEvent e)
		{
			UpdateElements();
		}

		// Update the padding elements when the element is attached to a panel.
		private void OnAttachToPanelEvent(AttachToPanelEvent e)
		{
			UpdateElements();
		}

		// Update the padding elements.
		public void UpdateElements()
		{
			var designRatio = (float)RatioWidth / RatioHeight;
			var currRatio = resolvedStyle.width / resolvedStyle.height;
			var diff = currRatio - designRatio;

			if (RatioWidth <= 0.0f || RatioHeight <= 0.0f)
			{
				leftPadding.style.width = 0;
				rightPadding.style.width = 0;
				Debug.LogError($"[AspectRatio] Invalid width:{RatioWidth} or height:{RatioHeight}");
				return;
			}

			if (float.IsNaN(resolvedStyle.width) || float.IsNaN(resolvedStyle.height))
			{
				return;
			}

			if (diff > 0.01f)
			{
				var w = (resolvedStyle.width - (resolvedStyle.height * designRatio)) * 0.5f;
				leftPadding.style.width = w;
				rightPadding.style.width = w;
			}
			else
			{
				leftPadding.style.width = 0;
				rightPadding.style.width = 0;
			}

			// Make sure the elements are showing correctly.
			leftPadding.SendToBack();
			rightPadding.BringToFront();
		}
	}
