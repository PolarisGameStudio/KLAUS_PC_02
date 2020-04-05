using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEntryList<T> : MonoBehaviour where T : MonoBehaviour
{
	public T prefab;
	public RectTransform container;
	public bool canResizeInX;
	public bool canResizeInY;

	protected List<T> currentItems = new List<T>();

    protected void InitPools(int initialPoolSize = 10)
    {
        ObjectPool.CreatePool<T>(prefab, initialPoolSize);
    }

	void OnEnable()
	{
		AdjustContainer();
	}

	public void Add(T clone, bool adjustInmediatly = true)
	{
		clone.transform.SetParent(container);
        clone.transform.localPosition = Vector3.zero;
		clone.transform.localScale = Vector3.one;
		currentItems.Add(clone);

		if (adjustInmediatly) AdjustContainer();
	}

	public void RemoveAll()
	{
		for (int i = 0; i != currentItems.Count; ++i)
			ClearEntry(currentItems[i]);

		currentItems.RemoveAll(t => true);
		AdjustContainer();
	}

	public void RemoveAt(int index)
	{
		if (0 <= index && index < currentItems.Count) {
			ClearEntry(currentItems[index]);
            currentItems.RemoveAt(index);

            Vector2 size = container.sizeDelta;
            if (canResizeInX) size.x = 0;
            if (canResizeInY) size.y = 0;

            container.sizeDelta = size;
		}
	}

    public T FindItem(Predicate<T> match)
    {
        return currentItems.Find(match);
    }

	public virtual void ClearEntry(T entry) { }

	public void AdjustContainer()
	{
		if (gameObject.activeInHierarchy) {
			StopCoroutine("AdjustContainerSize");
			StartCoroutine("AdjustContainerSize");
		}
	}

	IEnumerator AdjustContainerSize()
	{
		yield return null;
        AdjustContent();
	}

    public void AdjustContent()
    {
        Vector2 size = container.sizeDelta;

        if (container.childCount == 0) {
            size = Vector2.zero;
        } else {
            float width = 0, height = 0;

            for (int i = 0; i != container.childCount; ++i) {
                RectTransform child = container.GetChild(i).GetComponent<RectTransform>();

                Vector2 childSize = Vector3.Scale(child.sizeDelta, child.pivot);
                childSize.x += Mathf.Abs(child.anchoredPosition.x);
                childSize.y += Mathf.Abs(child.anchoredPosition.y);

                if (childSize.x > width)
                    width = childSize.x;

                if (childSize.y > height)
                    height = childSize.y;
            }

            if (canResizeInX)
                size.x = width;

            if (canResizeInY)
                size.y = height;
        }

        container.sizeDelta = size;
    }
}
