/*using UnityEngine;
using TMPro;

public class BallVisual : MonoBehaviour
{
    [Header("References")]
    public BallData ballData;

    public Renderer ballRenderer;

    public TextMeshPro numberText;

    [Header("Materials")]
    public Material whiteMaterial;
    public Material yellowMaterial;
    public Material blueMaterial;
    public Material redMaterial;
    public Material purpleMaterial;
    public Material orangeMaterial;
    public Material greenMaterial;
    public Material maroonMaterial;
    public Material blackMaterial;

    [Header("Stripe")]
    public GameObject stripeVisual;

    private void Start()
    {
        ApplyVisual();
    }

    public void ApplyVisual()
    {
        if (ballData == null)
            return;

        ApplyNumber();
        ApplyColor();
        ApplyStripe();
    }

    private void ApplyNumber()
    {
        if (numberText == null)
            return;

        if (ballData.ballType == BallType.CueBall)
        {
            numberText.gameObject.SetActive(false);
            return;
        }

        numberText.gameObject.SetActive(true);

        numberText.text =
            ballData.ballNumber.ToString();
    }

    private void ApplyColor()
    {
        if (ballRenderer == null)
            return;

        Material material = null;

        switch (ballData.ballNumber)
        {
            case 1:
            case 9:
                material = yellowMaterial;
                break;

            case 2:
            case 10:
                material = blueMaterial;
                break;

            case 3:
            case 11:
                material = redMaterial;
                break;

            case 4:
            case 12:
                material = purpleMaterial;
                break;

            case 5:
            case 13:
                material = orangeMaterial;
                break;

            case 6:
            case 14:
                material = greenMaterial;
                break;

            case 7:
            case 15:
                material = maroonMaterial;
                break;

            case 8:
                material = blackMaterial;
                break;
        }

        if (ballData.IsCueBall())
        {
            material = whiteMaterial;
        }

        if (material != null)
        {
            ballRenderer.material = material;
        }
    }

    private void ApplyStripe()
    {
        if (stripeVisual == null)
            return;

        stripeVisual.SetActive(
            ballData.ballType == BallType.Stripe
        );
    }
}*/

using UnityEngine;
using TMPro;

public class BallVisual : MonoBehaviour
{
    [Header("References")]
    public BallData ballData;
    public Renderer ballRenderer;
    public TextMeshPro numberText;

    [Header("Solid Materials")]
    public Material whiteMaterial;
    public Material yellowMaterial;
    public Material blueMaterial;
    public Material redMaterial;
    public Material purpleMaterial;
    public Material orangeMaterial;
    public Material greenMaterial;
    public Material maroonMaterial;
    public Material blackMaterial;

    [Header("Stripe Materials")]
    public Material stripeYellowMaterial;
    public Material stripeBlueMaterial;
    public Material stripeRedMaterial;
    public Material stripePurpleMaterial;
    public Material stripeOrangeMaterial;
    public Material stripeGreenMaterial;
    public Material stripeMaroonMaterial;

    private void Start()
    {
        ApplyVisual();
    }

    public void ApplyVisual()
    {
        if (ballData == null)
            return;

        ApplyNumber();
        ApplyColor();
    }

    private void ApplyNumber()
    {
        if (numberText == null)
            return;

        if (ballData.ballType == BallType.CueBall)
        {
            numberText.gameObject.SetActive(false);
            return;
        }

        numberText.gameObject.SetActive(true);

        numberText.text =
            ballData.ballNumber.ToString();
    }

    private void ApplyColor()
    {
        if (ballRenderer == null)
            return;

        Material material = null;

        int number =
            ballData.ballNumber;

        // Cue Ball
        if (ballData.IsCueBall())
        {
            material = whiteMaterial;
        }

        // Eight Ball
        else if (ballData.IsEightBall())
        {
            material = blackMaterial;
        }

        // Solid Balls: 1 - 7
        else if (ballData.IsSolid())
        {
            material = GetSolidMaterial(number);
        }

        // Stripe Balls: 9 - 15
        else if (ballData.IsStripe())
        {
            material = GetStripeMaterial(number);
        }

        if (material != null)
        {
            ballRenderer.material = material;
        }
    }

    private Material GetSolidMaterial(int number)
    {
        switch (number)
        {
            case 1:
                return yellowMaterial;

            case 2:
                return blueMaterial;

            case 3:
                return redMaterial;

            case 4:
                return purpleMaterial;

            case 5:
                return orangeMaterial;

            case 6:
                return greenMaterial;

            case 7:
                return maroonMaterial;

            default:
                return null;
        }
    }

    private Material GetStripeMaterial(int number)
    {
        switch (number)
        {
            case 9:
                return stripeYellowMaterial;

            case 10:
                return stripeBlueMaterial;

            case 11:
                return stripeRedMaterial;

            case 12:
                return stripePurpleMaterial;

            case 13:
                return stripeOrangeMaterial;

            case 14:
                return stripeGreenMaterial;

            case 15:
                return stripeMaroonMaterial;

            default:
                return null;
        }
    }
}