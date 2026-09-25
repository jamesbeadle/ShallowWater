# Refactor audit

Generated 2026-09-25 17:16 UTC.

## Headline

**Code quality score 84.8%.** **0 of 3 source files are over the 100-line limit (0.0%)**; the worst file is 0 lines.

## Code quality score

| Element | Reading | Score | Weight | 0% at |
| --- | --- | --- | --- | --- |
| **Standard baseline checks** | | **95.5%** | **36** | |
| Files over the line limit | 0 in 3 files | 100.0% | 10 | 50% of files |
| Worst file, in limits over | 0 | 100.0% | 5 | 9 |
| Functions over the line limit | not measured | not measured | — | 25% of functions |
| Else blocks | 0 in 12 branches | 100.0% | 5 | 50% of branches |
| Duplication % | not measured | not measured | — | 20 |
| Explanatory comment lines | 0 in 0.17 thousand lines | 100.0% | 4 | 50 per thousand lines |
| Inline magic values | 0 in 0.17 thousand lines | 100.0% | 4 | 20 per thousand lines |
| Orphan components and functions | not measured | not measured | — | 10% of components and functions |
| Long member chain lines | 0 in 0.17 thousand lines | 100.0% | 4 | 30 per thousand lines |
| Deeply indented lines | 2 in 0.17 thousand lines | 59.8% | 4 | 30 per thousand lines |
| Overlong function names | not measured | not measured | — | 10% of functions |
| **Design pattern file count** | | **not measured** | **0** | |
| Files the patterns predict but are missing | not measured | not measured | — | 50% of predicted files |
| Entities outside their expected file count | not measured | not measured | — | 50% of entities |
| **Prose** | | **57.1%** | **14** | |
| Conditions with calls tangled inside calls | 0 in 12 branches | 100.0% | 8 | 25% of branches |
| Conditions compared to a raw literal | 4 in 12 branches | 0.0% | 6 | 25% of branches |
| Accessor names that want to be a property | not measured | not measured | — | 10% of functions |
| **Widget adoption** | | **not measured** | **0** | |
| Markup written by hand where a widget should be | not measured | not measured | — | 50% of widget slots |
| **Input validation** | | **not measured** | **0** | |
| Doors that write without checking their input against the columns | not measured | not measured | — | 50% of write doors |

Each element scores 100% with no offenders and falls in a straight line to 0% when its offenders, measured against the size of the codebase, reach the figure in the last column. The score is the weighted average of the elements that could be measured; an element that could not be measured lends its weight to the rest. Weights and zero points are set in `tools/refactor/rules.json` under `score.elements`. The offenders behind every reading are in `tools/refactor/audit-output/audit.json`.

## The repository by area

| Area | Files | Of which audited source | Source lines |
| --- | --- | --- | --- |
| docs | 5 | 0 | 0 |
| infrastructure | 3 | 0 | 0 |
| **whole repository** | **8** | **0** | **0** |

## Summary

| Check | Key figures |
| --- | --- |
| fileLength | limit: 100, filesOverLimit: 0, totalFiles: 3, totalLines: 166, worstFileLines: 0, worstFileTimesOverLimit: 0.0 |
| functionShape | limit: 30, functionsOverLimit: 0, totalFunctions: 0, elseBlocks: 0, ifBlocks: 12, measurementIsHeuristic: True |
| functionNames | overlongFunctionNames: 0, maxWords: 5, maxLength: 40 |
| accessorNames | gluedAccessorNames: 0, measurementIsHeuristic: True |
| duplication | skipped: jscpd is not installed (npm install -g jscpd) |
| naming | bannedAbbreviationHits: 0, unprefixedBooleans: 0 |
| comments | explanatoryCommentLines: 0, filesWithComments: 0, taskMarkers: 0 |
| magicValues | inlineHexColours: 0, inlineStyleAttributes: 0, repeatedStringLiterals: 0 |
| prose | longMemberChainLines: 0, deeplyIndentedLines: 2, overlongLines: 0, measurementIsHeuristic: True |
| conditions | tangledConditionLines: 0, literalComparisonLines: 4, measurementIsHeuristic: True |
| orphans | orphanFunctions: 0, functionsExamined: 0 |
| designPatterns | roleFamilies: 0, predictedFiles: 0, predictedFilesMissing: 0, entities: 0, entitiesOutOfRange: 0, measurementIsHeuristic: True |
| inventory | pages: 0, components: 0, orphanComponents: 0, averagePageLines: 0 |
| siteDefinition | skipped: no siteDefinition catalogue in rules.json |
| inputValidation | schemaTables: 0, limitedColumns: 0, writeDoors: 0, unvalidatedDoors: 0, looserLimits: 0 |
| fileAreas | totalFiles: 8, docs: 5, infrastructure: 3 |

## Against the baseline

No `baseline.json` beside the audit — nothing to ratchet against.

## Worst files by length

All files are within the limit.

Full detail, including every offender list, is in `audit.json`.
