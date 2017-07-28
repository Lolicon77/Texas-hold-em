using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace L7 {
	public class VerticalText : Text {

		private readonly UIVertex[] m_TempVerts = new UIVertex[4];

		protected override void Awake() {
			base.Awake();
			this.horizontalOverflow = HorizontalWrapMode.Overflow;
			this.verticalOverflow = VerticalWrapMode.Overflow;
		}

		protected override void OnPopulateMesh(VertexHelper toFill) {
			if ((UnityEngine.Object)this.font == (UnityEngine.Object)null) {
				return;
			}
			this.m_DisableFontTextureRebuiltCallback = true;
			this.cachedTextGenerator.PopulateWithErrors(this.text, this.GetGenerationSettings(this.rectTransform.rect.size), this.gameObject);
			IList<UIVertex> verts = this.cachedTextGenerator.verts;
			float num1 = 1f / this.pixelsPerUnit;
			int num2 = verts.Count - 4;
			Vector2 point = new Vector2(verts[0].position.x, verts[0].position.y) * num1;
			Vector2 vector2 = this.PixelAdjustPoint(point) - point;
			toFill.Clear();
			if (vector2 != Vector2.zero) {
				for (int index1 = 0; index1 < num2; ++index1) {
					int index2 = index1 & 3;
					this.m_TempVerts[index2] = verts[index1];
					this.m_TempVerts[index2].position *= num1;
					this.m_TempVerts[index2].position.x += vector2.x;
					this.m_TempVerts[index2].position.y += vector2.y;
					if (index2 == 3)
						toFill.AddUIVertexQuad(this.m_TempVerts);
				}
			} else {
				if (fontSize == 0 || rectTransform.sizeDelta.x < fontSize || rectTransform.sizeDelta.y < fontSize) {
					return;
				}
				int charCount = num2 / 4 + 1;
				float w, h, posX, posY;
				Vector3 offset = Vector2.zero;
				float halfRectW = rectTransform.sizeDelta.x * 0.5f;
				float halfRectH = rectTransform.sizeDelta.y * 0.5f;
				posX = halfRectW - fontSize;
				posY = halfRectH;
				Vector3 currentPos;
				for (int i = 0; i < charCount; i++) {
					w = verts[i * 4 + 1].position.x - verts[i * 4].position.x;
					h = verts[i * 4 + 1].position.y - verts[i * 4 + 2].position.y;
					offset = new Vector2(fontSize - w, -fontSize + h) * 0.5f;
					if (-posY + fontSize > halfRectH) {
						posY = halfRectH;
						posX -= fontSize;
						if (-posX > halfRectW) {
							return;
						}
					}
					currentPos = new Vector3(posX, posY, 0);
					posY -= fontSize;
					for (int j = 0; j < 4; j++) {
						this.m_TempVerts[j] = verts[i * 4 + j];
					}
					this.m_TempVerts[0].position = currentPos + offset;
					this.m_TempVerts[1].position = this.m_TempVerts[0].position + Vector3.right * w;
					this.m_TempVerts[2].position = this.m_TempVerts[1].position - Vector3.up * h;
					this.m_TempVerts[3].position = this.m_TempVerts[0].position - Vector3.up * h;
					toFill.AddUIVertexQuad(this.m_TempVerts);
				}
			}
			this.m_DisableFontTextureRebuiltCallback = false;
		}
	}
}