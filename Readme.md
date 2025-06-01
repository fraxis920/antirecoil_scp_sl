•Simple Anti-Recoil 

This is a basic anti-recoil tool written in C# using visual studio code and .NET 6.0.
To use it, simply run the included .exe 


•Status

The project is functional, but weapon recoil patterns still need fine-tuning. I’m currently working on improving the accuracy of each weapon’s behavior.


•How to Adjust Recoil

For now, you can manually adjust the recoil by editing the following dictionary in the code:

// Format: "WeaponName", new PointF(x, y)
var recoilPatterns = new Dictionary<string, PointF>
{
    { "COM-18", new PointF(0f, 1.5f) },
    { "Crossvec", new PointF(0f, 2.2f) },
    { "FSP-9", new PointF(0f, 2.0f) },
    { "E-11-SR", new PointF(0f, 2.8f) },
    { "FRMG-0", new PointF(-3f, 8.5f) },
    { "AK", new PointF(0f, 3.7f) },
    { "Shotgun", new PointF(0f, 4.0f) },
    { "Logicer", new PointF(0f, 4.5f) }
};

•What the values mean:

X: Horizontal mouse movement (positive = right, negative = left)

Y: Vertical mouse movement (higher value = stronger downward pull)

Example: { "AK", new PointF(0f, 3.7f) } → no horizontal compensation, 3.7 pixels downward per shot.


