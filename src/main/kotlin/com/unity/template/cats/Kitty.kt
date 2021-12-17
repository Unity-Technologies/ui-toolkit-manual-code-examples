package com.unity.template.cats

import org.jetbrains.exposed.dao.id.IntIdTable
import org.jetbrains.exposed.sql.Column

const val DEFAULT_ID = -1

data class Kitty(val id: Int = DEFAULT_ID, val name: String, val color: String)

object Cats : IntIdTable() {
    val name: Column<String> = varchar("name", 100)
    val color: Column<String> = varchar("color", 100)
}
