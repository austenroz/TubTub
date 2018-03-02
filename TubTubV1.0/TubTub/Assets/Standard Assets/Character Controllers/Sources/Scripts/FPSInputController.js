private var motor : CharacterMotor;

private var s_InputMethod : String;
public var go_InputReference : GameObject;

// Use this for initialization
function Awake () {
    motor = GetComponent(CharacterMotor);
}

function Start()
{
    s_InputMethod = go_InputReference.name;
}

// Update is called once per frame
function Update () {
    // Get the input vector from keyboard or analog stick
    var directionVector = Vector3.zero;
    if (s_InputMethod == "Keyboard")
    {
        directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
    else
    {
        directionVector = new Vector3(Input.GetAxis(s_InputMethod + "Horizontal"), 0, Input.GetAxis(s_InputMethod + "Vertical"));
    }
    //var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	
    if (directionVector != Vector3.zero) {
        // Get the length of the directon vector and then normalize it
        // Dividing by the length is cheaper than normalizing when we already have the length anyway
        var directionLength = directionVector.magnitude;
        directionVector = directionVector / directionLength;
		
        // Make sure the length is no bigger than 1
        directionLength = Mathf.Min(1, directionLength);
		
        // Make the input vector more sensitive towards the extremes and less sensitive in the middle
        // This makes it easier to control slow speeds when using analog sticks
        directionLength = directionLength * directionLength;
		
        // Multiply the normalized direction vector by the modified length
        directionVector = directionVector * directionLength;
    }
	
    // Apply the direction to the CharacterMotor
    motor.inputMoveDirection = transform.rotation * directionVector;
    if (s_InputMethod == "Keyboard")
    {
        motor.inputJump = Input.GetButton("Jump");
    }
    else
    {
        motor.inputJump = Input.GetButton(s_InputMethod + "Jump");
    }
    //motor.inputJump = Input.GetButton("Jump");
}

// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")
