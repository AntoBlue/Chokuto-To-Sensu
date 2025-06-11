using UnityEngine;

public class MenuPositioner : MonoBehaviour
{
    [SerializeField]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject[] referenceObjects;
    private int positionIndex = 0;
    [SerializeField] private GameObject pointer;
    private YPositionMover pointerMover;
    void Start()
    {
        pointerMover = pointer.GetComponent<YPositionMover>();
    }

    void positionUp()
    {
        
        if (positionIndex == 0)
        {
            positionIndex = referenceObjects.Length - 1;
        }
        else
        {
            positionIndex--;
        }
        pointer.SetActive(true);
        pointerMover.SetYPositionByIndex(positionIndex);
            
    }
    
    void positionDown()
    {
        if (positionIndex == referenceObjects.Length - 1)
        {
            positionIndex = 0;
        }
        else
        {
            positionIndex++;
        }
        pointer.SetActive(true);
        pointerMover.SetYPositionByIndex(positionIndex);
    }
    
    public void PositionAtIndex(int index)
    {
        if (index > referenceObjects.Length - 1 || index < 0)
        {
            return;
        }
        else
        {
            positionIndex = index;
        }

        pointer.SetActive(true);
        pointerMover.SetYPositionByIndex(positionIndex);
    }

    public void DisablePointer()
    {
        pointer.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        //Inserire input
    }
}
