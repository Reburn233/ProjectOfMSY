using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//UI事件监听器
public class UIEventListener : EventTrigger 
{
    public delegate void VoidDelegate(GameObject go);
    public delegate void IntDelegate(GameObject go, int iValue);
    public delegate void BoolDelegate(GameObject go, bool isValue);
    public delegate void FloatDelegate(GameObject go, float fValue);
    public delegate void StringDelegate(GameObject go, string strValue);

    public VoidDelegate onClick;            //点击事件
    public IntDelegate onDropdownChange;    //下拉列表值改变
    public BoolDelegate onToggleChange;     //多选框值改变
    public FloatDelegate onSliderChange;    //滑动条值改变
    public FloatDelegate onScrollbarChange;  //滚动条值改变
    public StringDelegate onInputFieldChange; //输入域值改变

    public static UIEventListener Get(GameObject go) //提供给外部获得UIEventListener对象的接口
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null)
        {
            listener = go.AddComponent<UIEventListener>();
        }
        return listener;
    }
    //点击事件(Button、Toggle)
    public override void OnPointerClick(PointerEventData eventData) 
    {
        if(onClick != null)
        {
            onClick(gameObject);
        }
        if (onToggleChange != null)
        {
            onToggleChange(gameObject, gameObject.GetComponent<Toggle>().isOn);
        }
    }
	//拖拽事件(Slider、Scrollbar)
    public override void OnDrag(PointerEventData eventData)  
    {
        if (onSliderChange != null)
        {
            onSliderChange(gameObject, gameObject.GetComponent<Slider>().value);
        }
        if (onScrollbarChange != null)
        {
            onScrollbarChange(gameObject, gameObject.GetComponent<Scrollbar>().value);
        }
    }
	//选择事件(Dropdown)
    public override void OnSelect(BaseEventData eventData)  
    {
        if(onDropdownChange != null)
        {
            onDropdownChange(gameObject, gameObject.GetComponent<Dropdown>().value);
        }
    }
	//取消选择事件(InputField)
    public override void OnDeselect(BaseEventData eventData)  
    {
        if (onInputFieldChange != null)
        {
            onInputFieldChange(gameObject, gameObject.GetComponent<InputField>().text);
        }
    }


}
