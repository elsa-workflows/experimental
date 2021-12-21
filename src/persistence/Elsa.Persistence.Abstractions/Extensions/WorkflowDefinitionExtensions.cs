using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Elsa.Persistence.Entities;
using Elsa.Persistence.Models;
using LinqKit;

namespace Elsa.Persistence.Extensions;

public static class WorkflowDefinitionExtensions
{
    public static bool WithVersion(this WorkflowDefinition workflow, VersionOptions versionOptions)
    {
        var isPublished = workflow.IsPublished;
        var isLatest = workflow.IsLatest;
        var version = workflow.Version;

        if (versionOptions.IsDraft)
            return !isPublished;
        if (versionOptions.IsLatest)
            return isLatest;
        if (versionOptions.IsPublished)
            return isPublished;
        if (versionOptions.IsLatestOrPublished)
            return isPublished || isLatest;
        if (versionOptions.AllVersions)
            return true;
        if (versionOptions.Version > 0)
            return version == versionOptions.Version;
        return true;
    }

    public static IEnumerable<WorkflowDefinition> WithVersion(
        this IEnumerable<WorkflowDefinition> query,
        VersionOptions versionOptions) =>
        query.Where(x => x.WithVersion(versionOptions)).OrderByDescending(x => x.Version);

    public static IQueryable<WorkflowDefinition> WithVersion(
        this IQueryable<WorkflowDefinition> query,
        VersionOptions versionOptions) =>
        query.Where(x => x.WithVersion(versionOptions)).OrderByDescending(x => x.Version);

    public static Expression<Func<WorkflowDefinition, bool>> WithVersion(this Expression<Func<WorkflowDefinition, bool>> predicate, VersionOptions? version = default)
    {
        var versionOption = version ?? VersionOptions.Latest;

        if (versionOption.IsDraft)
            return predicate.And(x => !x.IsPublished);
        if (versionOption.IsLatest)
            return predicate.And(x => x.IsLatest);
        if (versionOption.IsPublished)
            return predicate.And(x => x.IsPublished);
        if (versionOption.IsLatestOrPublished)
            return predicate.And(x => x.IsPublished || x.IsLatest);
        if (versionOption.Version > 0)
            return predicate.And(x => x.Version == versionOption.Version);

        return predicate;
    }
}