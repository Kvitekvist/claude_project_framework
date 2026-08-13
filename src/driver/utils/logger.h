#pragma once

#include <string>
#include <fstream>
#include <mutex>
#include <ctime>

namespace vremulator {

class Logger {
public:
    enum class Level {
        Debug,
        Info,
        Warning,
        Error
    };

    static Logger& Instance();

    void Log(Level level, const std::string& message);
    void Debug(const std::string& message);
    void Info(const std::string& message);
    void Warning(const std::string& message);
    void Error(const std::string& message);

    void SetLogFile(const std::string& filename);
    void SetLevel(Level minLevel);

private:
    Logger();
    ~Logger();

    Logger(const Logger&) = delete;
    Logger& operator=(const Logger&) = delete;

    std::string GetTimestamp();
    std::string LevelToString(Level level);

    std::ofstream m_logFile;
    std::mutex m_mutex;
    Level m_minLevel;
    bool m_initialized;
};

// Convenience macros
#define LOG_DEBUG(msg) vremulator::Logger::Instance().Debug(msg)
#define LOG_INFO(msg) vremulator::Logger::Instance().Info(msg)
#define LOG_WARNING(msg) vremulator::Logger::Instance().Warning(msg)
#define LOG_ERROR(msg) vremulator::Logger::Instance().Error(msg)

} // namespace vremulator
