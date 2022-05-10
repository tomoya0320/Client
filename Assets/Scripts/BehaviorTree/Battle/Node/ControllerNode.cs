﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BehaviorTree.Battle {
	public abstract class ControllerNode : BehaviorNode {
		[Input(connectionType = ConnectionType.Override)]
		public BehaviorPort In;
		[Output]
		public BehaviorPort Out;
  }
}