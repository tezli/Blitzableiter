Blitzableiter
=============

About
-----

The Blitzableiter project was initiated in 2009 by Recurity Labs GmbH. The goal was to find a way to fight flash malware since the Flash runtime was unfixable and traditional detection mechanisms (AV/IDS) failed. The constant surfacing of new attacks against Flash requires a defense approach that doesn’t depend on attack signatures.

Blitzableiter is implemented in fully managed C#, targeting the .NET 2.0 runtime and is
binary compatible with the Microsoft CLR as well as Mono 1.2.

"Blitzableiter" is the German term for lightning rod, since it turns dangerous
lightning into a harmless flash


What it does
------------

The approach is to safely parse the complete SWF file, strictly verify all data structures against their specified properties, discard the original file and create a new, "normalized" SWF file for the final consumer (Normalization through Recreation).
It produces a non-malicious Flash file as output by verifying the inter-Tag consistency and potentially adjusting the AVM byte code.
Wellformed input files produce functionally equivalent output files.

For more information read this this [presentation](http://www.recurity-labs.com/content/pub/FX_Blitzableiter_BHUSA2010.pdf "PDF") from BlackHat USA 2010