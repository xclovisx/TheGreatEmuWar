//Unfortunately Unity 5.4f UI buttons do not interact with locked cursor, i hope they'll return this functionality in next updates.
//http://forum.unity3d.com/threads/fake-mouse-position-in-4-6-ui-answered.283748/

using UnityEngine;
using UnityEngine.EventSystems;

public class VRInputModule : StandaloneInputModule {
	// NEXT TWO FUNCTIONS (GetPointerData/CopyFromTo)
	// are copied from PointerInputModule source, next update for Unity this likely won't be necessary (As Tim C has made them protected as of this morning (my current version is 4.6.0f3), thank you!), if you have a later version, remove these two functions to receive updates!
	protected bool GetPointerData(int id, out PointerEventData data, bool create)
	{
		if (!m_PointerData.TryGetValue(id, out data) && create)
		{
			data = new PointerEventData(eventSystem)
			{
				pointerId = id,
			};
			m_PointerData.Add(id, data);
			return true;
		}
		return false;
	}

	private void CopyFromTo(PointerEventData @from, PointerEventData @to)
	{
		@to.position = @from.position;
		@to.delta = @from.delta;
		@to.scrollDelta = @from.scrollDelta;
		@to.pointerCurrentRaycast = @from.pointerCurrentRaycast;
	}

	// This is the real function we want, the two commented out lines (Input.mousePosition) are replaced with m_cursorPos (our fake mouse position, set with the public function, UpdateCursorPosition)
	private readonly MouseState m_MouseState = new MouseState();
	protected override MouseState GetMousePointerEventData( int id =0)
	{
		// Populate the left button...
		PointerEventData leftData;
		var created = GetPointerData( kMouseLeftId, out leftData, true );

		leftData.Reset();

		if (created)
		leftData.position = Input.mousePosition;
		Vector2 pos = new Vector2(Screen.width / 2, (Screen.height / 2)+5);
		leftData.delta = pos - leftData.position;
		leftData.position = pos;
		leftData.scrollDelta = Input.mouseScrollDelta;
		leftData.button = PointerEventData.InputButton.Left;
		eventSystem.RaycastAll(leftData, m_RaycastResultCache);
		var raycast = FindFirstRaycast(m_RaycastResultCache);
		leftData.pointerCurrentRaycast = raycast;
		m_RaycastResultCache.Clear();

		// copy the apropriate data into right and middle slots
		PointerEventData rightData;
		GetPointerData(kMouseRightId, out rightData, true);
		CopyFromTo(leftData, rightData);
		rightData.button = PointerEventData.InputButton.Right;

		PointerEventData middleData;
		GetPointerData(kMouseMiddleId, out middleData, true);
		CopyFromTo(leftData, middleData);
		middleData.button = PointerEventData.InputButton.Middle;

		m_MouseState.SetButtonState(PointerEventData.InputButton.Left, StateForMouseButton(0), leftData);
		m_MouseState.SetButtonState(PointerEventData.InputButton.Right, StateForMouseButton(1), rightData);
		m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, StateForMouseButton(2), middleData);

		return m_MouseState;
	}

}
