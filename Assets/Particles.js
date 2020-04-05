#pragma strict

function Start () {

            // Set the sorting layer of the particle system.
            GetComponent.<ParticleSystem>().GetComponent.<Renderer>().sortingLayerName = "foreground";
            GetComponent.<ParticleSystem>().GetComponent.<Renderer>().sortingOrder = 2;
        
    

}

function Update () {

}