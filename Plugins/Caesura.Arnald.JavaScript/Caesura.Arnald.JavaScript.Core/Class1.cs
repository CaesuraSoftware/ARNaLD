using System;

namespace Caesura.Arnald.JavaScript.Core
{
    // TODO: 
    // - Implement and deploy V8:
    //   https://github.com/rjamesnw/v8dotnet
    //   Unfortunately we can't use Microsoft.ClearScript because it's Windows-specific
    //   and according to them would be a large undertaking to make portable.
    //   v8DotNet doesn't have Linux binaries either, so it may even be that we'll
    //   be forced to use Jint or Jurassic. TypeScript should still work if we only
    //   target ES5.
    //   Jiny 3.x (beta) seems like the best choice, as despite being beta and having
    //   potentially breaking changes, it's already used in production.
    // - Add optional TypeScript mode:
    //   https://stackoverflow.com/questions/19023498/loading-the-typescript-compiler-into-clearscript-wscript-is-undefined-imposs
    // 
    
    public class Class1
    {
    }
}
