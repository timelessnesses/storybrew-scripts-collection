using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Numerics;
using System.Collections.Generic;
namespace StorybrewScripts
{
    public class GlowOverlay : StoryboardObjectGenerator
    {

        [Configurable]
        public int FadeDuration = 200;

        [Configurable]
        public string SpritePath = "sb/glow.png";

        [Configurable]
        public double SpriteScale = 1;
	
	[Configurable]
	public int StartTime = 0;

	[Configurable]
	public int EndTime = 0;

        public override void Generate()
        {
            Generates(StartTime,EndTime);
        }
        public void Generates(int StartTime, int EndTime)
        {
            var blur = GetMapsetBitmap(SpritePath);
            var hitobjectLayer = GetLayer("");
            foreach (var hitobject in Beatmap.HitObjects)
            {
                if ((StartTime != 0 || EndTime != 0) && 
                    (hitobject.StartTime < StartTime - 5 || EndTime - 5 <= hitobject.StartTime))
                    continue;
                var the_blur = hitobjectLayer.CreateSprite(SpritePath, OsbOrigin.Centre);
                the_blur.Scale(OsbEasing.OutBounce, hitobject.StartTime, hitobject.EndTime + FadeDuration, SpriteScale, SpriteScale * 1);
                the_blur.Fade(OsbEasing.In, hitobject.StartTime, hitobject.EndTime + FadeDuration, 1, 0);
                the_blur.Additive(hitobject.StartTime, hitobject.EndTime + FadeDuration);
                the_blur.Color(hitobject.StartTime, hitobject.Color);
            }
        }
    }
}
