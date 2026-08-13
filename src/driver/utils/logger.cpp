#include "logger.h"
#include <iostream>
#include <sstream>
#include <iomanip>

#ifdef _WIN32
#include <windows.h>
#endif

namespace vremulator {

Logger& Logger::Instance() {
    static Logger instance;
    return instance;
}

Logger::Logger() : m_minLevel(Level::Debug), m_initialized(false) {
    // Default log file location - will be in driver directory
    SetLogFile("driver_vremulator.log");
}

Logger::~Logger() {
    if (m_logFile.is_open()) {
        LOG_INFO("Logger shutting down");
        m_logFile.close();
    }
}

void Logger::SetLogFile(const std::string& filename) {
    std::lock_guard<std::mutex> lock(m_mutex);

    if (m_logFile.is_open()) {
        m_logFile.close();
    }

    m_logFile.open(filename, std::ios::out | std::ios::app);

    if (m_logFile.is_open()) {
        m_initialized = true;
        Log(Level::Info, "VREmulator driver logger initialized");
    }
}

void Logger::SetLevel(Level minLevel) {
    m_minLevel = minLevel;
}

void Logger::Log(Level level, const std::string& message) {
    if (level < m_minLevel) {
        return;
    }

    std::lock_guard<std::mutex> lock(m_mutex);

    std::string logLine = GetTimestamp() + " [" + LevelToString(level) + "] " + message;

    // Write to file
    if (m_logFile.is_open()) {
        m_logFile << logLine << std::endl;
        m_logFile.flush();
    }

    // Also output to debugger on Windows
#ifdef _WIN32
    OutputDebugStringA((logLine + "\n").c_str());
#endif

    // And to console for debugging
    std::cout << logLine << std::endl;
}

void Logger::Debug(const std::string& message) {
    Log(Level::Debug, message);
}

void Logger::Info(const std::string& message) {
    Log(Level::Info, message);
}

void Logger::Warning(const std::string& message) {
    Log(Level::Warning, message);
}

void Logger::Error(const std::string& message) {
    Log(Level::Error, message);
}

std::string Logger::GetTimestamp() {
    auto now = std::time(nullptr);
    auto tm = *std::localtime(&now);

    std::ostringstream oss;
    oss << std::put_time(&tm, "%Y-%m-%d %H:%M:%S");
    return oss.str();
}

std::string Logger::LevelToString(Level level) {
    switch (level) {
    case Level::Debug:   return "DEBUG";
    case Level::Info:    return "INFO ";
    case Level::Warning: return "WARN ";
    case Level::Error:   return "ERROR";
    default:             return "UNKNOWN";
    }
}

} // namespace vremulator
