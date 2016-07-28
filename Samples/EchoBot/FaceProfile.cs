using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Bot.DiversityInclusionBot
{
    public class Age
    {
        public int range { get; set; }
        public int value { get; set; }
    }

    public class Gender
    {
        public double confidence { get; set; }
        public string value { get; set; }
    }

    public class Glass
    {
        public double confidence { get; set; }
        public string value { get; set; }
    }

    public class PitchAngle
    {
        public double value { get; set; }
    }

    public class RollAngle
    {
        public double value { get; set; }
    }

    public class YawAngle
    {
        public double value { get; set; }
    }

    public class Pose
    {
        public PitchAngle pitch_angle { get; set; }
        public RollAngle roll_angle { get; set; }
        public YawAngle yaw_angle { get; set; }
    }

    public class Race
    {
        public double confidence { get; set; }
        public string value { get; set; }
    }

    public class Smiling
    {
        public double value { get; set; }
    }

    public class Attribute
    {
        public Age age { get; set; }
        public Gender gender { get; set; }
        public Glass glass { get; set; }
        public Pose pose { get; set; }
        public Race race { get; set; }
        public Smiling smiling { get; set; }
    }

    public class Center
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class EyeLeft
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class EyeRight
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class MouthLeft
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class MouthRight
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Nose
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Position
    {
        public Center center { get; set; }
        public EyeLeft eye_left { get; set; }
        public EyeRight eye_right { get; set; }
        public double height { get; set; }
        public MouthLeft mouth_left { get; set; }
        public MouthRight mouth_right { get; set; }
        public Nose nose { get; set; }
        public double width { get; set; }
    }

    public class Face
    {
        public Attribute attribute { get; set; }
        public string face_id { get; set; }
        public Position position { get; set; }
        public string tag { get; set; }
    }

    public class FaceApiWebResponsePayload
    {
        public List<Face> face { get; set; }
        public int img_height { get; set; }
        public string img_id { get; set; }
        public int img_width { get; set; }
        public string session_id { get; set; }
        public string url { get; set; }
    }
}