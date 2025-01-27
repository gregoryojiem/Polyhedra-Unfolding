# Efficient Polyhedra Unfolder & Net Generator

This project is about the problem of unfolding convex polyhedra. 

There is a [famous open problem](https://topp.openproblem.net/p9) in computational geometry that asks:
*Can every convex polyhedron be cut along its edges and unfolded flat into a single, non-overlapping, simple polygon?*

Currently, the answer is unknown.

While researching that open problem, my teammate and I noticed that few good tools were publicly available for efficiently constructing the nets of large polyhedra. This repo is a cleaned-up version of the library we ended up writing.

## Features
- **Pretty Efficient**: Uses an O(n log n) algorithm to incrementally construct nets from polygonal faces with relatively quick geometric operations, backtracking when necessary. 
- **Very scalable!**: Can generate nets for polyhedra with over 100,000 faces.
- **Web Demo**: A blog post/interactive demo showing off this project is available at: https://gregoryojiem.github.io/polyhedra-unfolding/
- **Standalone Application**: Includes a terminal application for generating nets and visualizing them. The terminal application can be run by importing the NetGenerator project and calling 'dotnet run'. 

- **Library**: Contains all the classes necessary for converting polyhedra into nets

## Example output
![Original Image](https://github.com/gregoryojiem/polyhedra-unfolding/blob/main/docs/figure6.png?raw=true)

*An example of a generated net for a polyhedron with 10,000 faces.*

## Remaining Work
- **User-Defined Polyhedra**: Add support for inputting user-defined polyhedra. Currently, we have a large list of well-known polyhedra hard-coded in. It wouldn't be difficult to also allow for input files that specify vertices/faces.
- **Algorithmic Improvements**:
  - Implement a **greedy heuristic** to improve efficiency. Wolfram Schlickenrieder's paper, *Nets of Polyhedra*, provides interesting insights into potential heuristics.
  - Implement **GPU-based intersection checking** when generating polygon nets. GPUs excel at this so it should give a fairly large boost.

## Credits

The below libraries were used in this project :)
- **ASP.NET** for the website framework.
- **MIConvexHull** and **RBush** for some of the core library functionality.
- **ImageSharp** and **CommandLineParser** for the NetGenerator project.

Research-related attribution can be found in the GitHub page link in the bibliography section.