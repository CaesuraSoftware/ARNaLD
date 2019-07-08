
## LEGAL

"Contributing" denotes merge requests and reports filed in this repository's issues section.

When contributing to this project, you agree under no uncertain conditions that you shall never pursue litigation in any form, including through lawsuits or serving notices of copyright claim or violation, on Caesura Software Solutions, it's contributors, employees and affiliates. You agree to relinquish all rights and ownership of any and all resources you contribute to the project, be it source code, image files, or any other item within your legal ownership that is merged to this project's repository. You agree that any resources you are contributing to the project are legally copywritten under your name and you hold ownership of all items included. Should you be publishing an item for a third party, this third party agrees to relinquish all copyright ownership of all merged items to the project as well, in addition to never seeking litigation or to issue copyright violation notices to Caesura Software Solutions, it's contributors, employees and affiliates. If said third party denies this, your contributions will be reverted and you will be barred from the project pending internal investigation.

If required, you may submit a request as a bug report that you wish to regain ownership or retain co-ownership of your contributing items. Please explicitly list all items you wish to regain ownership of in your request.

As a policy, Caesura Software Solutions will never seek litigation or issue copyright violation notices if you continue to use the items you have relinquished copyright and ownership over in any other project or fork, provided you do not charge for them. If a project you wish to contribute an item to has a similar ownership relinquishing policy as this one, and you wish to submit said item to both this project and the other ownership-relinquishing project, we are preemptively giving them co-ownership of the items in question, as long as they agree to giving Caesura Software Solutions co-ownership of said items in return. Note that all parties involved with copyright retention and co-ownership must also agree to never seek litigation or issue copyright violation notices against Caesura Software Solutions, it's contributors, employees and affiliates, or else this preemptive co-ownership authorization is revoked and nullified.

You agree under no uncertain conditions that Caesura Software Solutions staff holds exclusive rights to prevent you from contributing to the project or communicating with Caesura Software Solutions staff for any amount of time under any conditions or circumstances. Attempting to circumvent these decisions will result in a harassment claim being levied against your person.

## Prerequisites

Contributing to this project requires complete compliance with the [Caesura Code of Conduct](CONDUCT.md). Failure to comply will bar you from this project indefinitely. You will not regain ownership or copyright of any contributions you merge with this project. As a rule of thumb, treat this repository and it's issues sections as a scientific project, not a social network.

## Coding Convention

Largely, the project follows the [Microsoft Visual C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions), with the one exception that core type aliases cannot be used. All instances of `int`, `string`, `object`, etcetera, must instead be `Int32`, `String`, `Object`, etcetera. Use a conversion tool if you disagree with this convention that strongly (we may supply you with a tool to convert to and from the aliased types in the future, as we do sympathize. Until then, a few console commands should suffice). If you attempt to merge a file with these aliases in-tact, the merge will be denied and we will politely remind you of this policy so you can re-submit the patch with the complying types. Thank you for having patience with this policy.

 - Absolutely 100% of all non-static fields, properties and methods must be prefixed with `this.`. If `this.` can be used, it must be used.

 - We prefer interfaces over abstract classes. However, if the inheriting object should contain more than two properties or a callback, and more than one or two classes will implement it, it is advised to use an abstract class instead. If making a generic interface or abstract class, always make an accompanying non-generic variant, for use in collection types.
 
 - Private backing fields for properties must be named after their respective property and affixed with "b_", "b" for "backing". Exmaple, the property "MyProperty" is backed by the backing field "b_MyProperty". If a property's accessor implements more than one line of code, implement it as a private method and use the lambda syntax to call it within the property. Example: `MyProperty { get => this.b_MyProperty; set => this.SetMyProperty(value); }`
 
 - Do not use ValueTuples or Tuples for method returns. Either use a full custom class/struct or simply use "out" arguments.
 
 - Default Interface members are strictly disallowed.
 
 - `if` statements with an `else`/`else if` clause under them must have an empty comment before them (`/**/ if (...)`) to align them with the rest of the clauses.
 
 - If a class has more than two arguments, instead make a configuration class to pass to it. The configuration class should be defined in the same file as the class, above the class's definition. The configuration class must be configured with defaults.
 
 - If a class implements an interface specifically for making that class testable or replacable, implement the interface above the class in it's source file, instead of putting it in it's own source file. Interfaces designed for multiple classes should have their own source file. If a new class is implementing an interface in a class' source file, move that interface into it's own source file.

 - Use of `goto` is allowed. LINQ and proper refactoring virtually eliminates any need for a `goto`, but that does not mean `goto` does not have it's uses (In fact, we suggest every contributor use at least one `goto` somewhere, if for nothing else than to irritate people).

 - Try to keep use of `switch` statements down to a minimum. Especially in the case of C#, a `switch` statement is a sure-fire sign of poor design. That is not to say they are outright disallowed, rather, it is advised you first consider a collection type, an if-else chain, or similar extendable solution. If a `switch` makes sense, use it.

 - The `using System;` declaration must be on top of the file, outside of the namespace declaration. All additional `using` directives must be directly under the namespace declaration.

 - Never embed strings or magic numbers in code in a method. They should always be stored in a property or static constructor.

 - All files must begin and end with a newline.
 
 - Liberal usage of swear words is allowed, but not outright encouraged. We feel this enhances the contribution experience.
 
 - If in doubt, look at the project's source code.
