using UnityEngine;

public interface IBodyPart
{
    BodyPart bodyPart {get; set;}
}

public class Body : MonoBehaviour, IBodyPart
{
    [SerializeField]
    private BodyPart _bodyPart;
    public BodyPart bodyPart
    {
        get {return _bodyPart;}
        set {_bodyPart = value;}
    }
}

public enum BodyPart {
    Body,
    Head,
    Legs,
    Arms,
    Tail,
    Wings
}