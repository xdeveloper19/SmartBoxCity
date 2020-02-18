package crc645b68eef9a2a04271;


public class Data
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SmartBoxCity.Model.OrderViewModel.Data, SmartBoxCity", Data.class, __md_methods);
	}


	public Data ()
	{
		super ();
		if (getClass () == Data.class)
			mono.android.TypeManager.Activate ("SmartBoxCity.Model.OrderViewModel.Data, SmartBoxCity", "", this, new java.lang.Object[] {  });
	}

	public Data (java.lang.String p0, java.lang.String p1, java.lang.String p2)
	{
		super ();
		if (getClass () == Data.class)
			mono.android.TypeManager.Activate ("SmartBoxCity.Model.OrderViewModel.Data, SmartBoxCity", "System.String, mscorlib:System.String, mscorlib:System.String, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
