using UnityEngine;

public class Gun : Offable
{
    Rotatable rotatable;

    protected override void Start()
    {
        base.Start();
        rotatable = GetComponent<Rotatable>();
    }

    void Update()
    {
        if (!IsOn)
            return;
    }

    public override bool TryOff()
    {
        rotatable.CanRotate = false;
        return base.TryOff();
    }

    public override void On()
    {
        base.On();
        rotatable.CanRotate = true;
    }
}
