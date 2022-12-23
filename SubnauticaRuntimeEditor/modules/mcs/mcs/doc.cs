//
// doc.cs: Support for XML documentation comment.
//
// Authors:
//	Atsushi Enomoto <atsushi@ximian.com>
//  Marek Safar (marek.safar@gmail.com>
//
// Dual licensed under the terms of the MIT X11 or GNU GPL
//
// Copyright 2004 Novell, Inc.
// Copyright 2011 Xamarin Inc
//
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;

namespace Mono.CSharp
{
	//
	// Implements XML documentation generation.
	//
	class DocumentationBuilder
	{
		readonly ModuleContainer module;
		readonly ModuleContainer doc_module;
		
		static readonly string line_head = Environment.NewLine + "            ";
		

		ParserSession session;

		public DocumentationBuilder (ModuleContainer module)
		{

		}

		Report Report {
			get {
				return module.Compiler.Report;
			}
		}

		public MemberName ParsedName {
			get; set;
		}

		public List<DocumentationParameter> ParsedParameters {
			get; set;
		}

		public TypeExpression ParsedBuiltinType {
			get; set;
		}

		public Operator.OpType? ParsedOperator {
			get; set;
		}

		//
		// Generates xml doc comments (if any), and if required,
		// handle warning report.
		//
		internal void GenerateDocumentationForMember (MemberCore mc)
		{
			
		}



		//
		// Outputs XML documentation comment from tokenized comments.
		//
		public bool OutputDocComment (string asmfilename, string xmlFileName)
		{
			return true;
		}
	}

	//
	// Type lookup of documentation references uses context of type where
	// the reference is used but type parameters from cref value
	//
	sealed class DocumentationMemberContext : IMemberContext
	{
		readonly MemberCore host;
		MemberName contextName;

		public DocumentationMemberContext (MemberCore host, MemberName contextName)
		{
			this.host = host;
			this.contextName = contextName;
		}

		public TypeSpec CurrentType {
			get {
				return host.CurrentType;
			}
		}

		public TypeParameters CurrentTypeParameters {
			get {
				return contextName.TypeParameters;
			}
		}

		public MemberCore CurrentMemberDefinition {
			get {
				return host.CurrentMemberDefinition;
			}
		}

		public bool IsObsolete {
			get {
				return false;
			}
		}

		public bool IsUnsafe {
			get {
				return host.IsStatic;
			}
		}

		public bool IsStatic {
			get {
				return host.IsStatic;
			}
		}

		public ModuleContainer Module {
			get {
				return host.Module;
			}
		}

		public string GetSignatureForError ()
		{
			return host.GetSignatureForError ();
		}

		public ExtensionMethodCandidates LookupExtensionMethod (string name, int arity)
		{
			return null;
		}

		public FullNamedExpression LookupNamespaceOrType (string name, int arity, LookupMode mode, Location loc)
		{
			if (arity == 0) {
				var tp = CurrentTypeParameters;
				if (tp != null) {
					for (int i = 0; i < tp.Count; ++i) {
						var t = tp[i];
						if (t.Name == name) {
							t.Type.DeclaredPosition = i;
							return new TypeParameterExpr (t, loc);
						}
					}
				}
			}

			return host.Parent.LookupNamespaceOrType (name, arity, mode, loc);
		}

		public FullNamedExpression LookupNamespaceAlias (string name)
		{
			throw new NotImplementedException ();
		}
	}

	class DocumentationParameter
	{
		public readonly Parameter.Modifier Modifier;
		public FullNamedExpression Type;
		TypeSpec type;

		public DocumentationParameter (Parameter.Modifier modifier, FullNamedExpression type)
			: this (type)
		{
			this.Modifier = modifier;
		}

		public DocumentationParameter (FullNamedExpression type)
		{
			this.Type = type;
		}

		public TypeSpec TypeSpec {
			get {
				return type;
			}
		}

		public void Resolve (IMemberContext context)
		{
			type = Type.ResolveAsType (context);
		}
	}
}
