using UnityEngine;
using System.Collections;

public class PlayerTextTrigger : TextTrigger
{
    public PlayersID TypePlayer = PlayersID.Player1Klaus;

    protected override bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player") && other.GetComponent<PlayerInfo>().playerType == TypePlayer;
    }

}
