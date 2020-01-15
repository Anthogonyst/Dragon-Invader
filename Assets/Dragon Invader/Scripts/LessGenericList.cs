using System;

namespace DragonInvader {
public class LessGenericList {
	public string callName { get; set; }
	public bool callActive { get; set; }
	public int callVal { 
		get { return _callVal; }
		set {
			if (clamp) {
				if (value >= lowerClamp && value <= upperClamp)
					_callVal = value;
				else _callVal = -1;
			} else _callVal = value;
		}
	}
	public bool clamp {
		get { return _clamp; }
		set {
			if (!(lowerClamp == -1 && upperClamp == -1))
				_clamp = value;
		}
	}
	
	private int _callVal = -1;	
	private int lowerClamp = -1;
	private int upperClamp = -1;
	private bool _clamp = false;
	
	public override bool Equals(object obj) {
		var item = obj as LessGenericList;
		
		if (item == null)
			return false;
		
		return this.callName.Equals(item.callName);
	}
	
	public override int GetHashCode() {
		return this.callName.GetHashCode();
	}
	
	public bool MatchName(string s) {
		if (s == callName)
			return true;
		else return false;
	}
	
	public bool ModifyClamps(int a, int b) {
		lowerClamp = a;
		upperClamp = b;
		_clamp = true;
		return _clamp;
	}
	
	public LessGenericList(string s) {
		callName = s;
		callVal = -1;
		callActive = false;
	}
	
	public LessGenericList(string s, bool z) {
		callName = s;
		callVal = -1;
		callActive = z;
	}
	
	public LessGenericList(string s, int a, int b) {
		callName = s;
		callVal = -1;
		callActive = false;
		lowerClamp = a;
		upperClamp = b;
		_clamp = true;
	}
	
	public LessGenericList(string s, int n, bool z) {
		callName = s;
		callVal = n;
		callActive = z;
	}
	
	public LessGenericList(string s, int n, bool z, int a, int b) {
		callName = s;
		callVal = n;
		callActive = z;
		lowerClamp = a;
		upperClamp = b;
		_clamp = true;
	}
	
}
}