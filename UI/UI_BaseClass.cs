public abstract class UI_BaseClass : DebuggableBaseClass
{
    protected UI_MenuController uI_MenuController;

    // you can put the container object field here as well

    protected virtual void Awake()
    {
        uI_MenuController = transform.parent.GetComponent<UI_MenuController>();

        GetObjectsAndButtons();
        AddListeners();
    }

    // Toggle Container method should be added

    protected abstract void GetObjectsAndButtons();

    protected abstract void AddListeners();
}

