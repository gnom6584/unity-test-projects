using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultSoldierView : SoldierView
{
    public Renderer Renderer;

    public Material Red;

    public Material Blue;

    public override void OnTeamChanged(Team team)
    {
        if (team.Color == Color.blue) 
            Renderer.material = Blue;
        else if(team.Color == Color.red)
            Renderer.material = Red;
    }

}
