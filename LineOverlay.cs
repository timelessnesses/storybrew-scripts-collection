using System;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Collections.Generic;
namespace StorybrewScripts
{
    public class LineOverlay : StoryboardObjectGenerator
    {

        [Configurable]
        public int FadeDuration = 200;

        [Configurable]
        public string SpritePath = "sb/bar.png";

        [Configurable]
        public double SpriteScale = 1;

        [Configurable]
        public int StartTime = 0;

        [Configurable]
        public int EndTime = 0;

        [Configurable]
        public string Type = "X";

        public override void Generate()
        {
            if(Type == "X"){
                GenerateX(StartTime,EndTime);
            }
            else if(Type == "Y"){
                GenerateY(StartTime,EndTime);
            }
            else if(Type == "XY"){
                GenerateX(StartTime,EndTime);
                GenerateY(StartTime,EndTime);
            }
            else{
                throw new System.Exception("Invalid Type");
            }
        }

        public void GenerateX(int StartTime, int EndTime)
        {
            var hitobjectLayer = GetLayer("");
            foreach (var hitobject in Beatmap.HitObjects)
            {
                if ((StartTime != 0 || EndTime != 0) &&
                    (hitobject.StartTime < StartTime - 5 || EndTime - 5 <= hitobject.StartTime))
                    continue;

                var hSprite = hitobjectLayer.CreateSprite(SpritePath, OsbOrigin.Centre, hitobject.Position);
                hSprite.Fade(OsbEasing.None, hitobject.StartTime-FadeDuration, hitobject.StartTime-200,0 ,1);
                hSprite.ScaleVec(OsbEasing.In, hitobject.StartTime-FadeDuration, hitobject.StartTime, new OpenTK.Vector2((int)0, (int)0), new OpenTK.Vector2((int)SpriteScale, 1000));
                hSprite.ScaleVec(OsbEasing.In, hitobject.EndTime, hitobject.EndTime + FadeDuration, new OpenTK.Vector2((int)SpriteScale, 1000), new OpenTK.Vector2(0, 1000));
                hSprite.Fade(OsbEasing.In, hitobject.EndTime, hitobject.EndTime + FadeDuration, 1, 0);
                hSprite.Additive(hitobject.StartTime, hitobject.EndTime + FadeDuration);
                hSprite.Color(hitobject.StartTime, hitobject.Color);

                if (hitobject is OsuSlider)
                {
                    hSprite.MoveX(OsbEasing.InOutSine, hitobject.EndTime + FadeDuration, hitobject.EndTime + (FadeDuration * 1.2), hitobject.Position.X, hitobject.Position.X + Random(-2, 2));
                    var timestep = Beatmap.GetTimingPointAt((int)hitobject.StartTime).BeatDuration / 128;
                    var startTime = hitobject.StartTime;
                    while (true)
                    {
                        var endTime = startTime + timestep;

                        var complete = hitobject.EndTime - endTime < 5;
                        if (complete) endTime = hitobject.EndTime;

                        var startPosition = hSprite.PositionAt(startTime);
                        hSprite.MoveX(startTime, endTime, startPosition.X, hitobject.PositionAtTime(endTime).X);

                        if (complete) break;
                        startTime += timestep;
                    }
                }
            }
        }

        public void GenerateY(int StartTime, int EndTime)
        {
            var hitobjectLayer = GetLayer("");
            foreach (var hitobject in Beatmap.HitObjects)
            {
                if ((StartTime != 0 || EndTime != 0) &&
                    (hitobject.StartTime < StartTime - 5 || EndTime - 5 <= hitobject.StartTime))
                    continue;

                var hSprite = hitobjectLayer.CreateSprite(SpritePath, OsbOrigin.Centre, hitobject.Position);
                hSprite.Fade(OsbEasing.None, hitobject.StartTime-FadeDuration, hitobject.StartTime-200,0 ,1);
                hSprite.ScaleVec(OsbEasing.In, hitobject.StartTime-FadeDuration, hitobject.StartTime, new OpenTK.Vector2((int)0, (int)0), new OpenTK.Vector2(1000, (int)SpriteScale));
                hSprite.ScaleVec(OsbEasing.In, hitobject.EndTime, hitobject.EndTime + FadeDuration, new OpenTK.Vector2(1000, (int)SpriteScale), new OpenTK.Vector2(1000, 0));
                hSprite.Fade(OsbEasing.In, hitobject.EndTime, hitobject.EndTime + FadeDuration, 1, 0);
                hSprite.Additive(hitobject.StartTime, hitobject.EndTime + FadeDuration);
                hSprite.Color(hitobject.StartTime, hitobject.Color);

                if (hitobject is OsuSlider)
                {
                    hSprite.MoveX(OsbEasing.InOutSine, hitobject.EndTime + FadeDuration, hitobject.EndTime + (FadeDuration * 1.2), hitobject.Position.X, hitobject.Position.X + Random(-2, 2));
                    var timestep = Beatmap.GetTimingPointAt((int)hitobject.StartTime).BeatDuration / 1024;
                    var startTime = hitobject.StartTime;
                    while (true)
                    {
                        var endTime = startTime + timestep;

                        var complete = hitobject.EndTime - endTime < 5;
                        if (complete) endTime = hitobject.EndTime;

                        var startPosition = hSprite.PositionAt(startTime);
                        hSprite.MoveY(startTime, endTime, startPosition.Y, hitobject.PositionAtTime(endTime).Y);

                        if (complete) break;
                        startTime += timestep;
                    }
                }
            }
        }
        
    }
}