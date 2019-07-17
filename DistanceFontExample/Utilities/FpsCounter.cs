using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceFontExample.Utilities
{
	public class FpsCounter
	{
		//How many data points should be stored for average FPS calculation
		private const int dataPoints = 100;
		//How often average FPS should be updated (without this the fps counter updates unreadably fast)
		private const float updateInterval = 0.2f;

		private List<float> fpsHistory;
		private float nextUpdate;

		public int AverageFps { get; private set; }

		public FpsCounter()
		{
			fpsHistory = new List<float>(dataPoints);
		}

		public void Reset()
		{
			fpsHistory.Clear();
			nextUpdate = 0;
		}

		public void Update(GameTime gameTime)
		{
			//If elapsed time is 0, we're unable to record an FPS value
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (elapsed == 0) return;

			//Record value and only storing a certain number of data points
			fpsHistory.Add(1 / elapsed);
			if (fpsHistory.Count > dataPoints)
				fpsHistory.RemoveAt(0);

			float total = (float)gameTime.TotalGameTime.TotalSeconds;
			if (total >= nextUpdate)
			{
				//Update average FPS
				float fps = 0;
				for (int i = 0; i < fpsHistory.Count; i++)
					fps += fpsHistory[i];

				AverageFps = (int)(fps / fpsHistory.Count);
				nextUpdate = total + updateInterval;
			}
		}
	}
}
