package com.unity.template.monitoring

import io.micrometer.core.instrument.Counter
import io.micrometer.core.instrument.Gauge
import io.micrometer.core.instrument.Timer
import io.micrometer.core.instrument.MeterRegistry
import java.util.function.Supplier

abstract class MicrometerMonitor(private val meterRegistry: MeterRegistry) {
    protected fun getGauge(metric: MonitoredMetric, valueSupplier: Supplier<Number>): Gauge {
        return getGauge(metric, valueSupplier, meterRegistry)
    }

    protected fun getCounter(metric: MonitoredMetric): MicrometerCounter {
        return MicrometerCounter(metric, meterRegistry)
    }

    protected fun getTimer(metric: MonitoredMetric): Timer {
        return getTimer(metric, meterRegistry)
    }
}

class MonitoredMetric(val title: String, val description: String, vararg val tagPairs: TagPair) {
    data class TagPair(val label: String, val value: String)
}

class MicrometerCounter(private val metric: MonitoredMetric, private val meterRegistry: MeterRegistry) {
    fun increment() {
        registerCounter(listOf()).increment()
    }

    fun increment(dynamicTags: List<MonitoredMetric.TagPair>) {
        registerCounter(dynamicTags).increment()
    }

    private fun registerCounter(metricFilters: List<MonitoredMetric.TagPair>): Counter {
        return Counter
            .builder(metric.title)
            .description(metric.description)
            .apply {
                metric.tagPairs.asList().forEach { this.tag(it.label, it.value) }
            }
            .apply {
                metricFilters.forEach { this.tag(it.label, it.value) }
            }
            .register(meterRegistry)
    }
}

fun getGauge(metric: MonitoredMetric, valueSupplier: Supplier<Number>, meterRegistry: MeterRegistry): Gauge {
    return Gauge
        .builder(metric.title, valueSupplier)
        .description(metric.description)
        .register(meterRegistry)
}

fun getTimer(metric: MonitoredMetric, meterRegistry: MeterRegistry): Timer {
    return Timer
        .builder(metric.title)
        .description(metric.description)
        .publishPercentiles(0.5, 0.9, 0.99)
        .register(meterRegistry)
}
