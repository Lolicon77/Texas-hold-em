using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace L7
{
	public class ShowFPSOnUGUICanvas : MonoBehaviour {


		// Attach this to a GUIText to make a frames/second indicator.
		//
		// It calculates frames/second over each updateInterval,
		// so the display does not keep changing wildly.
		//
		// It is also fairly accurate at very low FPS counts (<10).
		// We do this not by simply counting frames per interval, but
		// by accumulating FPS for each frame. This way we end up with
		// correct overall FPS even if the interval renders something like
		// 5.5 frames.


		public float updateInterval = 0.5F;


		private float accum = 0; // FPS accumulated over the interval
		private int frames = 0; // Frames drawn over the interval
		private float timeleft; // Left time for current interval


		public Text text;


		void Start() {
			timeleft = updateInterval;
		}


		void Update() {
			timeleft -= Time.deltaTime;
			accum += Time.timeScale / Time.deltaTime;
			++frames;


			// Interval ended - update GUI text and start new interval
			if (timeleft <= 0.0) {
				// display two fractional digits (f2 format)
				float fps = accum / frames;
				string format = System.String.Format("fps:{0:F2}", fps);
				text.text = format;


				if (fps < 30)
					text.color = Color.yellow;
				else
					if (fps < 10)
						text.color = Color.red;
					else
						text.color = Color.green;
				//	DebugConsole.Log(format,level);
				timeleft = updateInterval;
				accum = 0.0F;
				frames = 0;
			}
		}
	}


}
