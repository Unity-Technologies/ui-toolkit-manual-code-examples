package com.unity.template.utils

import java.util.ArrayList

/**
 * Find the root cause exception by looping through all the causes until it reaches the root.
 * Avoids infinite loops when handling recursive causes.
 *
 * Logic copied from
 * https://github.com/apache/commons-lang/blob/master/src/main/java/org/apache/commons/lang3/exception/ExceptionUtils.java
 */
fun getRootCause(throwable: Throwable): Throwable {
    val list = ArrayList<Throwable>()
    var rootCause: Throwable? = throwable
    while (rootCause != null && !list.contains(rootCause)) {
        list.add(rootCause)
        rootCause = rootCause.cause
    }
    return list.lastOrNull() ?: throwable
}
